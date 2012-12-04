using System.Collections.Generic;
using System.Linq;
using Sicemed.Web.Models.ViewModels.Listados;

namespace Sicemed.Web.Infrastructure.Queries.Listados
{
    public interface IListadoObraSocialesQuery : IQuery<List<ListadoObraSocialesViewModel>> { }

    public class ListadoObraSocialesQuery : Query<List<ListadoObraSocialesViewModel>>, IListadoObraSocialesQuery
    {
        protected override List<ListadoObraSocialesViewModel> CoreExecute()
        {
            var profesionalesDb = SessionFactory.GetCurrentSession().QueryOver<Models.ObraSocial>().OrderBy(p => p.RazonSocial).Asc.List();

            return profesionalesDb.Select(p => new ListadoObraSocialesViewModel
            {
                Nombre = p.RazonSocial,
                Planes = p.Planes
            }).ToList();
        }
    }
}