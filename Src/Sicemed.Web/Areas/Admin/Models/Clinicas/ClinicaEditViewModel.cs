using System;

namespace Sicemed.Web.Areas.Admin.Models.Clinicas
{
    public class ClinicaEditViewModel
    {
        public virtual long Id { get; set; }

        public virtual string RazonSocial { get; set; }

        public virtual int DocumentoTipoDocumentoValue { get; set; }
        public virtual string DocumentoTipoDocumentoDisplayName { get; set; }
        public virtual long DocumentoNumero { get; set; }

        public virtual string DomicilioDireccion { get; set; }
        public virtual long DomicilioLocalidadId { get; set; }
        public virtual string DomicilioLocalidadNombre { get; set; }
        public virtual double DomicilioLatitud { get; set; }
        public virtual double DomicilioLongitud { get; set; }


        public virtual string Email { get; set; }
        public virtual TimeSpan DuracionTurnoPorDefecto { get; set; }
        public virtual DateTime HorarioMatutinoDesde { get; set; }
        public virtual DateTime HorarioMatutinoHasta { get; set; }
        public virtual DateTime? HorarioVespertinoDesde { get; set; }
        public virtual DateTime? HorarioVespertinoHasta { get; set; }

        public virtual int NumeroInasistenciasConsecutivasGeneranBloqueo { get; set; }
        public virtual string GoogleMapsKey { get; set; }
    }
}