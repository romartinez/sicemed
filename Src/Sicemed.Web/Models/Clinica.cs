using System;
using Iesi.Collections.Generic;
using Sicemed.Web.Models.Components;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Models
{
    public class Clinica : Entity
    {
        private readonly ISet<Telefono> _telefonos;
        private readonly ISet<DayOfWeek> _diasHabilitados;

        public Clinica()
        {
            _telefonos = new HashedSet<Telefono>();
            _diasHabilitados = new HashedSet<DayOfWeek>();
        }

        public virtual string RazonSocial { get; set; }
        public virtual Documento Documento { get; set; }
        public virtual Domicilio Domicilio { get; set; }

        public virtual ISet<Telefono> Telefonos
        {
            get { return _telefonos; }
        }

        public virtual ISet<DayOfWeek> DiasHabilitados
        {
            get { return _diasHabilitados; }
        }

        public virtual string Email { get; set; }
        public virtual TimeSpan DuracionTurnoPorDefecto { get; set; }
        public virtual TimeSpan HorarioMatutinoDesde { get; set; }
        public virtual TimeSpan HorarioMatutinoHasta { get; set; }
        public virtual TimeSpan? HorarioVespertinoDesde { get; set; }
        public virtual TimeSpan? HorarioVespertinoHasta { get; set; }

        public virtual bool EsHorarioCorrido
        {
            get { return !HorarioVespertinoDesde.HasValue && !HorarioVespertinoHasta.HasValue; }
        }

        public virtual int NumeroInasistenciasConsecutivasGeneranBloqueo { get; set; }
        public virtual string GoogleMapsKey { get; set; }
        public virtual ISet<ObraSocial> ObrasSocialesAtendidas { get; set; }
        public virtual ISet<Paciente> Pacientes { get; set; }
        public virtual ISet<Profesional> Profesionales { get; set; }
        public virtual ISet<Secretaria> Secretarias { get; set; }
        public virtual ISet<Clinica> Partners { get; set; }

        public virtual Clinica AgregarTelefono(Telefono telefono)
        {
            if (telefono == null) throw new ArgumentNullException("telefono");

            _telefonos.Add(telefono);

            return this;
        }

        public virtual Clinica QuitarTelefono(Telefono telefono)
        {
            if (telefono == null) throw new ArgumentNullException("telefono");

            _telefonos.Remove(telefono);

            return this;
        }        
        
        public virtual Clinica AgregarDiaHabilitado(DayOfWeek dia)
        {
            _diasHabilitados.Add(dia);

            return this;
        }

        public virtual Clinica QuitarDiaHabilitado(DayOfWeek dia)
        {
            _diasHabilitados.Remove(dia);

            return this;
        }
    }
}