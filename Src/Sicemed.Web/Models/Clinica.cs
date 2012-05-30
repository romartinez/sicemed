using System;
using Iesi.Collections.Generic;
using Sicemed.Web.Models.Components;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Models
{
    public class Clinica : Entity
    {
        private readonly ISet<Telefono> _telefonos;

        public Clinica()
        {
            _telefonos = new HashedSet<Telefono>();
        }

        public virtual string RazonSocial { get; set; }
        public virtual Documento Documento { get; set; }
        public virtual Domicilio Domicilio { get; set; }

        public virtual ISet<Telefono> Telefonos
        {
            get { return _telefonos; }
        }

        public virtual string Email { get; set; }
        public virtual TimeSpan DuracionTurnoPorDefecto { get; set; }
        public virtual DateTime HorarioMatutinoDesde { get; set; }
        public virtual DateTime HorarioMatutinoHasta { get; set; }
        public virtual DateTime? HorarioVespertinoDesde { get; set; }
        public virtual DateTime? HorarioVespertinoHasta { get; set; }

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
    }
}