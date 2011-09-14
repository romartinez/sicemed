using System;
using System.Text;
using System.Web.Mvc;
using Castle.Core.Logging;
using Sicemed.Web.Infrastructure.ActionResults;
using Sicemed.Web.Infrastructure.Exceptions;
using Sicemed.Web.Infrastructure.Services;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Controllers
{
    public class BaseController : Controller
    {
        private ILogger _logger;

        public ILogger Logger
        {
            get { return _logger ?? (_logger = NullLogger.Instance); }
            set { _logger = value; }
        }

        public IMembershipService MembershipService {get;set;}

        protected new Persona User
        {
            get
            {
                return MembershipService.GetCurrentUser();   
            }
        }

        protected Persona Persona
        {
            get
            {
                return User;
            }
        }
        
        protected virtual T RetrieveParameter<T>(string paramName, string paramNameDescription = null, bool allowNulls = false)
        {
            if (string.IsNullOrWhiteSpace(paramName)) throw new ArgumentNullException("paramName");

            var providerResult = this.ValueProvider.GetValue(paramName);
            if(allowNulls && providerResult == null) return default(T);

            if (providerResult == null) 
                throw new ValidationErrorException(string.Format("No se encontró el valor para el parámetro: '{0}'", 
                    paramNameDescription ?? paramName));
            try
            {
                return (T)providerResult.ConvertTo(typeof(T));
            }
            catch (Exception ex)
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
    }
}