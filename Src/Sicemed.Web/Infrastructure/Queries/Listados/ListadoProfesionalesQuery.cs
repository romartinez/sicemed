using System.Collections.Generic;
using System.Linq;
using Sicemed.Web.Models.ViewModels.Listados;

namespace Sicemed.Web.Infrastructure.Queries.Listados
{
    public interface IListadoProfesionalesQuery : IQuery<List<ListadoProfesionalViewModel>> { }

    public class ListadoProfesionalesQuery : Query<List<ListadoProfesionalViewModel>>, IListadoProfesionalesQuery
    {
        protected override List<ListadoProfesionalViewModel> CoreExecute()
        {
            var profesionalesDb = SessionFactory.GetCurrentSession().QueryOver<Models.Especialidad>().OrderBy(p => p.Nombre).Asc.List();

            return profesionalesDb.Select(p => new ListadoProfesionalViewModel
            {

                IdEspecialidad = p.Id,
                Especialidad = p.Nombre,
                Profesional = p.Profesionales
            }).ToList();
        }
    }
}