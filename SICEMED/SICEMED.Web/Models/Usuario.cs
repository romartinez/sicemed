using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

using Sicemed.Web.Models.Components;
using Sicemed.Web.Models.Components.Documentos;

namespace Sicemed.Web.Models
{
    public class Usuario : Entity
    {
        #region Primitive Properties

        public virtual DateTime? FechaNacimiento { get; set; }

        public virtual Documento Documento { get; set; }
        
        public virtual Domicilio Domicilio { get; set; }

        public virtual bool? EstaHabilitadoTurnosWeb { get; set; }

        public virtual Telefono Telefono { get; set; }

        public virtual int InasistenciasContinuas { get; set; }

        public virtual string NumeroAfiliado { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ISet<Turno> Turnos { get; set; }

        #endregion

        public virtual string NombreUsuario { get; set; }

        public virtual string Nombre { get; set; }
        public virtual string SegundoNombre { get; set; }
        public virtual string Apellido{ get; set; }
        
        public virtual string Email { get; set; }
    }
}