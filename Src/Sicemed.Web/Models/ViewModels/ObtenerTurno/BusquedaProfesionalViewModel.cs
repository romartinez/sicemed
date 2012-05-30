using System;
using System.Collections.Generic;
using System.Linq;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Models.ViewModels.ObtenerTurno
{
    [Serializable]
    public class BusquedaProfesionalViewModel
    {
        public long Id { get; set; }
        public string Foto { get; set; }
        public string Nombre { get; set; }
        public TurnoViewModel ProximoTurnoLibre { get; set; }
        public IEnumerable<InfoViewModel> Especialidades { get; set; }

        public static BusquedaProfesionalViewModel Create(Persona persona)
        {
            var profesional = persona.As<Profesional>();
            var vm = new BusquedaProfesionalViewModel
                         {
                             Id = profesional.Id,
                             Nombre = persona.NombreCompleto,                             
                             Especialidades = profesional.Especialidades.Select(
                                     e => new InfoViewModel { Descripcion = e.Nombre, Id = e.Id })
                         };
            return vm;
        }
    }
}