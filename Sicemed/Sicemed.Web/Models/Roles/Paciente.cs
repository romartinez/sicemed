﻿using System;
using System.Collections.Generic;

namespace Sicemed.Web.Models.Roles
{
    public class Paciente : Rol
    {
        private readonly ISet<Turno> _turnos;

        protected Paciente()
        {
            _turnos = new HashSet<Turno>();
        }

        public override string DisplayName
        {
            get { return PACIENTE; }
        }

        public virtual string NumeroAfiliado { get; set; }

        public virtual Plan Plan { get; set; }

        public virtual int InasistenciasContinuas { get; set; }

        public virtual bool EstaHabilitadoTurnosWeb { get; set; }

        public virtual ISet<Turno> Turnos
        {
            get { return _turnos; }
        }

        public static Rol Create(string numeroAfiliado)
        {
            return new Paciente
                   {
                       NumeroAfiliado = numeroAfiliado,
                       EstaHabilitadoTurnosWeb = true,
                       InasistenciasContinuas = 0
                   };
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