using System;
using System.Collections.Generic;

namespace Sicemed.Web.Models.Roles
{
    public class Paciente : Rol
    {
        public override string DisplayName
        {
            get { return PACIENTE; }
        }

        public virtual string NumeroAfiliado { get; set; }
        
        public virtual Plan Plan { get; set; }

        public virtual int InasistenciasContinuas { get; set; }

        public virtual bool EstaHabilitadoTurnosWeb { get; set; }

        protected Paciente()
        {
            _turnos = new HashSet<Turno>();
        }

        public static Rol Create(string numeroAfiliado)
        {
            return new Paciente()
                   {
                       NumeroAfiliado = numeroAfiliado, 
                       EstaHabilitadoTurnosWeb = true, 
                       InasistenciasContinuas = 0
                   };
        }

        private ISet<Turno> _turnos;
        public virtual ISet<Turno> Turnos
        {
            get { return _turnos; }
        }

        public virtual Paciente AgregarTurno(Turno turno)
        {
            if (turno == null) throw new ArgumentNullException("turno");

            _turnos.Add(turno);
            turno.Paciente = this;

            return this;
        }
    }
}