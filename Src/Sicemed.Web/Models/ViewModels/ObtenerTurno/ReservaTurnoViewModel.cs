using System;

namespace Sicemed.Web.Models.ViewModels.ObtenerTurno
{
    [Serializable]
    public class ReservaTurnoViewModel
    {
        public long Id { get; set; }
        public DateTime FechaTurnoInicial { get; set; }

        public InfoViewModel Consultorio { get; set; }
        public InfoViewModel Paciente { get; set; }
        public InfoViewModel Especialidad { get; set; }
        public InfoViewModel Profesional { get; set; }

        public static ReservaTurnoViewModel Create(Turno turno)
        {
            var vm = new ReservaTurnoViewModel
                         {
                             Id = turno.Id,
                             FechaTurnoInicial = turno.FechaTurno,
                             Consultorio =
                                 new InfoViewModel() { Id = turno.Consultorio.Id, Descripcion = turno.Consultorio.Descripcion },
                             Paciente =
                                 new InfoViewModel() { Id = turno.Paciente.Id, Descripcion = turno.Paciente.Persona.NombreCompleto },
                             Especialidad =
                                 new InfoViewModel() { Id = turno.Especialidad.Id, Descripcion = turno.Especialidad.Nombre },
                             Profesional =
                                 new InfoViewModel() { Id = turno.Profesional.Id, Descripcion = turno.Profesional.Persona.NombreCompleto }
                         };
            return vm;
        }
    }
}