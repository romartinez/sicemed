using System.ComponentModel.DataAnnotations;
using Iesi.Collections.Generic;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Models
{
    public class Especialidad : Entity
    {
        #region Primitive Properties

        [Requerido]
        public virtual string Nombre { get; set; }
        [LargoCadenaPorDefecto]
        public virtual string Descripcion { get; set; }

        #endregion

        private ISet<Profesional> _profesionales;
        public virtual ISet<Profesional> Profesionales
        {
            get { return _profesionales; }
        }

        public Especialidad()
        {
            _profesionales = new HashedSet<Profesional>();
        }
    }
}