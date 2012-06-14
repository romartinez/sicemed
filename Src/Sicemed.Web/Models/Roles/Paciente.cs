using System;
using Iesi.Collections.Generic;

namespace Sicemed.Web.Models.Roles
{
    public class Paciente : Rol
    {
        private readonly ISet<Turno> _turnos;

        protected Paciente()
        {
            _turnos = new HashedSet<Turno>();
        }

        public override string DisplayName
        {
            get { return PACIENTE; }
        }

        public virtual string NumeroAfiliado { get; set; }

        public virtual Plan Plan { get; set; }

        public virtual int InasistenciasContinuas { get; protected set; }

        public virtual bool EstaHabilitadoTurnosWeb(int inasistenciasContinuasAdmitidas)
        {
            return InasistenciasContinuas < inasistenciasContinuasAdmitidas;
        }

        public virtual void ResetInasistencias()
        {
            InasistenciasContinuas = 0;
        }

        public virtual ISet<Turno> Turnos
        {
            get { return _turnos; }
        }

        public static Paciente Create(string numeroAfiliado)
        {
            return new Paciente
                   {
                       NumeroAfiliado = numeroAfiliado,                       
                       InasistenciasContinuas = 0
                   };
        }

        public virtual Paciente AgregarTurno(Turno turno)
        {
            if (turno == null) throw new ArgumentNullException("turno");

            _turnos.Add(turno);

            return this;
        }
    }
}