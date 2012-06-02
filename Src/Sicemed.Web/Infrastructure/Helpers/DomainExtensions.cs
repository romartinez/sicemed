using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NHibernate;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Enumerations;
using Sicemed.Web.Models.Enumerations.Documentos;

namespace Sicemed.Web.Infrastructure.Helpers
{
    public static class DomainExtensions
    {
        public static IEnumerable<SelectListItem> GetTiposDocumentos(int? selectedValue = null)
        {
            return Enumeration.GetAll<TipoDocumento>().Select(x =>
                new SelectListItem()
                    {
                        Value = x.Value.ToString(),
                        Text = x.DisplayName,
                        Selected = selectedValue.HasValue && x.Value == selectedValue.Value
                    });
        }

        public static IEnumerable<SelectListItem> GetProvincias(ISessionFactory sessionFactory, long? selectedValue = null)
        {
            var provincias = sessionFactory.GetCurrentSession().QueryOver<Provincia>().OrderBy(x => x.Nombre).Asc.Future();
            return provincias.Select(x =>
                new SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Nombre,
                    Selected = selectedValue.HasValue && x.Id == selectedValue.Value
                });
        }

        public static IEnumerable<SelectListItem> GetLocalidades(ISessionFactory sessionFactory, long provinciaId, long? selectedValue = null)
        {
            var localidades = sessionFactory.GetCurrentSession().QueryOver<Localidad>().OrderBy(x => x.Nombre).Asc.
                JoinQueryOver(l=>l.Provincia).Where(p=>p.Id == provinciaId).Future();
            return localidades.Select(x =>
                new SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Nombre,
                    Selected = selectedValue.HasValue && x.Id == selectedValue.Value
                });
        }
    }
}