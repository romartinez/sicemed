using System.Collections.Generic;
using Sicemed.Web.Models.Components;

namespace Sicemed.Web.Models.Roles
{
    public class Profesional : Rol
    {
        public override string DisplayName
        {
            get { return PROFESIONAL; }
        }

        private ISet<Especialidad> _especialidades;
        public virtual ISet<Especialidad> Especialidades
        {
            get { return _especialidades; }
        }

        private ISet<Agenda> _agendas;
        public virtual ISet<Agenda> Agendas
        {
            get { return _agendas; }
        }
        
        public virtual string Matricula { get; set; }

        protected Profesional()
        {
            _especialidades = new HashSet<Especialidad>();
            _agendas = new HashSet<Agenda>();
        }

        public static Rol Create(string matricula)
        {
            return new Profesional() { Matricula = matricula };
        }

    }
}