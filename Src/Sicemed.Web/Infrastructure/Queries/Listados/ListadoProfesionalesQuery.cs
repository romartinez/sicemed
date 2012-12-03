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
            var profesionalesDb = SessionFactory.GetCurrentSession().QueryOver<Models.Roles.Profesional>().List();

            return profesionalesDb.Select(p => new ListadoProfesionalViewModel
            {
                Nombre = p.Persona.NombreCompleto,
                Especialidades = p.Especialidades.Select(e => e.Nombre).ToList()
            }).ToList();
        }
    }
}