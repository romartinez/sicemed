using System;

namespace Sicemed.Web.Models
{
    public class AuditLog
    {        
        public virtual Guid Id { get; set; }
        public virtual long EntidadId { get; set; }
        public virtual string Entidad { get; set; }
        public virtual string Usuario { get; set; }
        public virtual string Accion { get; set; }
        public virtual DateTime Fecha { get; set; }
        public virtual string EntidadAntes { get; set; }
        public virtual string EntidadDespues { get; set; }
    }
}