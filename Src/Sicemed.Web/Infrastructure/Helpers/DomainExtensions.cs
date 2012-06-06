using System;
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

        public static IEnumerable<SelectListItem> GetObrasSociales(ISessionFactory sessionFactory, long? selectedValue = null)
        {
            var obrasSociales = sessionFactory.GetCurrentSession().QueryOver<ObraSocial>().OrderBy(x => x.RazonSocial).Asc.Future();
            return obrasSociales.Select(x =>
                new SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.RazonSocial,
                    Selected = selectedValue.HasValue && x.Id == selectedValue.Value
                });
        }

        public static IEnumerable<SelectListItem> GetPlanesObraSocial(ISessionFactory sessionFactory, long obraSocialId, long? selectedValue = null)
        {
            var planes = sessionFactory.GetCurrentSession().QueryOver<Plan>().OrderBy(x => x.Nombre).Asc.
                JoinQueryOver(l => l.ObraSocial).Where(p => p.Id == obraSocialId).Future();
            return planes.Select(x =>
                new SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Nombre,
                    Selected = selectedValue.HasValue && x.Id == selectedValue.Value
                });
        }

        public static IEnumerable<SelectListItem> GetEspecialidades(ISessionFactory sessionFactory, long[] selectedValues = null)
        {
            var selectedIds = selectedValues ?? new long[] {};
            var especialidades = sessionFactory.GetCurrentSession().QueryOver<Especialidad>().OrderBy(x => x.Nombre).Asc.Future();
            return especialidades.Select(x =>
                new SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Nombre,
                    Selected = selectedIds.Contains(x.Id)
                });
        }

        public static IEnumerable<SelectListItem> GetConsultorios(ISessionFactory sessionFactory, long? selectedValue = null)
        {
            var consultorios = sessionFactory.GetCurrentSession().QueryOver<Consultorio>()
                .OrderBy(x => x.Nombre).Asc.Future();
            return consultorios.Select(x =>
                new SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Nombre,
                    Selected = selectedValue.HasValue && x.Id == selectedValue.Value
                });
        }    
    }
}