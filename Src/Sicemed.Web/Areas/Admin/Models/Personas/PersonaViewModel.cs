using System;

namespace Sicemed.Web.Areas.Admin.Models.Personas
{
    public class PersonaViewModel
    {
        public virtual long Id { get; set; }
        public virtual string NombreCompleto { get; set; }
        public virtual string MembershipEmail { get; set; }
        public virtual DateTime? FechaNacimiento { get; set; }
        public virtual string DocumentoNumero { get; set; }
        public virtual string DocumentoTipoDocumentoDisplayName { get; set; }
        public virtual string DomicilioDireccion { get; set; }
        public virtual string DomicilioLocalidadNombre { get; set; }
        public virtual string DomicilioLocalidadProvinciaNombre { get; set; }
        public virtual string TelefonoPrefijo { get; set; }
        public virtual string[] Roles { get; set; }
        public virtual string TelefonoNumero { get; set; }
        public virtual bool MembershipIsLockedOut { get; set; }
    }
}