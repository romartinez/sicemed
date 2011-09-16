using System.Collections.Generic;

namespace Sicemed.Web.Models.Roles
{
    public class Profesional : Rol
    {
        private readonly ISet<Agenda> _agendas;
        private readonly ISet<Especialidad> _especialidades;

        protected Profesional()
        {
            _especialidades = new HashSet<Especialidad>();
            _agendas = new HashSet<Agenda>();
        }

        public override string DisplayName
        {
            get { return PROFESIONAL; }
        }

        public virtual ISet<Especialidad> Especialidades
        {
            get { return _especialidades; }
        }

        public virtual ISet<Agenda> Agendas
        {
            get { return _agendas; }
        }

        public virtual string Matricula { get; set; }

        public static Rol Create(string matricula)
        {
            return new Profesional {Matricula = matricula};
        }
    }
}