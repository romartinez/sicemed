using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Castle.Core.Logging;
using Sicemed.Web.Infrastructure.ActionResults;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Exceptions;
using Sicemed.Web.Infrastructure.Providers.Cache;
using Sicemed.Web.Infrastructure.Queries;
using Sicemed.Web.Infrastructure.Queries.Domain;
using Sicemed.Web.Infrastructure.Services;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Enumerations;
using Sicemed.Web.Models.Enumerations.Documentos;

namespace Sicemed.Web.Infrastructure.Controllers
{
    [AjaxHandleError]
    public class BaseController : Controller
    {
        private ILogger _logger;

        public ILogger Logger
        {
            get { return _logger ?? (_logger = NullLogger.Instance); }
            set { _logger = value; }
        }

        public IMembershipService MembershipService { get; set; }
        public IQueryFactory QueryFactory { get; set; }

        public ICacheProvider Cache { get; set; }

        protected new Persona User
        {
            get { return MembershipService.GetCurrentUser(); }
        }

        protected Persona Persona
        {
            get { return User; }
        }

        protected virtual T RetrieveParameter<T>(string paramName, string paramNameDescription = null,
                                                 bool allowNulls = false)
        {
            if (string.IsNullOrWhiteSpace(paramName)) throw new ArgumentNullException("paramName");

            var providerResult = this.ValueProvider.GetValue(paramName);
            if (allowNulls && providerResult == null) return default(T);

            if (providerResult == null)
                throw new ValidationErrorException(string.Format("No se encontró el valor para el parámetro: '{0}'",
                                                                 paramNameDescription ?? paramName));
            try
            {
                return (T) providerResult.ConvertTo(typeof (T));
            } catch (Exception ex)
            {
                var errorMsg = string.Format("El valor ingresado: '{0}' no es válido para para el campo {1}.",
                                             providerResult.AttemptedValue, paramNameDescription ?? paramName);

                if (Logger.IsWarnEnabled) Logger.WarnFormat(errorMsg + " Exc: " + ex.Message);

                throw new ValidationErrorException(errorMsg);
            }
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding,
                                           JsonRequestBehavior behavior)
        {
            return new NewtonJsonResult(base.Json(data, contentType, contentEncoding, behavior));
        }

        public const string MESSAGES_KEY = "MESSAGES_";
        protected void ShowMessages(params ResponseMessage[] messages)
        {
            var storedMessages = TempData.ContainsKey(MESSAGES_KEY)
                                                       ? (List<ResponseMessage>) TempData[MESSAGES_KEY]
                                                       : new List<ResponseMessage>();
            storedMessages.AddRange(messages);

            TempData[MESSAGES_KEY] = storedMessages;
        }

        #region Common Queries
        protected virtual IEnumerable<SelectListItem> GetConsultorios(long? selectedValue)
        {
            var query = QueryFactory.Create<IObtenerConsultoriosDropDownQuery>();
            query.SelectedValue = selectedValue;
            return query.Execute();
        }

        protected virtual IEnumerable<SelectListItem> GetEspecialidades(params long[] selectedValues)
        {
            var query = QueryFactory.Create<IObtenerEspecialidadesDropDownQuery>();
            query.SelectedValues = selectedValues;
            return query.Execute();
        }

        protected virtual IEnumerable<SelectListItem> GetLocalidadesPorProvincia(long provinciaId, long? selectedValue)
        {
            var query = QueryFactory.Create<IObtenerLocalidadesPorProvinciaDropDownQuery>();
            query.ProvinciaId = provinciaId;
            query.SelectedValue = selectedValue;
            return query.Execute();
        }

        protected virtual IEnumerable<SelectListItem> GetObrasSociales(long? selectedValue)
        {
            var query = QueryFactory.Create<IObtenerObrasSocialesDropDownQuery>();
            query.SelectedValue = selectedValue;
            return query.Execute();
        }


        protected virtual IEnumerable<SelectListItem> GetPlanesPorObraSocial(long obraSocialId, long? selectedValue)
        {
            var query = QueryFactory.Create<IObtenerPlanesPorObraSocialDropDownQuery>();
            query.ObraSocialId= obraSocialId;
            query.SelectedValue = selectedValue;
            return query.Execute();
        }

        protected virtual IEnumerable<SelectListItem> GetProvincias(long? selectedValue)
        {
            var query = QueryFactory.Create<IObtenerProvinciasDropDownQuery>();
            query.SelectedValue = selectedValue;
            return query.Execute();
        }

        protected virtual IEnumerable<SelectListItem> GetTiposDocumentos(long? selectedValue)
        {
            return Enumeration.GetAll<TipoDocumento>().Select(x =>
                new SelectListItem()
                {
                    Value = x.Value.ToString(),
                    Text = x.DisplayName,
                    Selected = selectedValue.HasValue && x.Value == selectedValue.Value
                });
        }

        protected virtual IEnumerable<SelectListItem> GetDiasSemana(DayOfWeek[] selectedValues)
        {
            var enumType = typeof (DayOfWeek);
            var values = Enum.GetValues(typeof (DayOfWeek)).Cast<DayOfWeek>();
            var converter = TypeDescriptor.GetConverter(enumType);

            var items = values.Select(v =>
            {
                var value = converter.ConvertToString(v);
                if (value.Equals(v.ToString()))
                {
                    //El converter no hizo nada pruevo con el resource
                    var rex = Properties.Resources.ResourceManager.GetString(string.Format("{0}_{1}", enumType.Name, value));
                    value = !string.IsNullOrWhiteSpace(rex)
                                ? rex
                                : value;
                }
                return new SelectListItem
                {
                    Text = value,
                    Value = v.ToString(),
                    Selected = selectedValues != null && selectedValues.Contains(v)
                };
            });

            return items;
        }        
        #endregion
    }
}