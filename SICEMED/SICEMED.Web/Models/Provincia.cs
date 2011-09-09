


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sicemed.Web.Models
{
    public class Provincia : Entity
    {
        public Provincia ()
        {
            _localidades = new HashSet<Localidad>();
        }

        #region Primitive Properties

        [Required]
        public virtual string Nombre { get; set; }

        #endregion

        #region Navigation Properties

        private ISet<Localidad> _localidades;
        public virtual IEnumerable<Localidad> Localidades
        {
            get { return _localidades; }
        }

        #endregion

        public virtual Provincia AgregarLocalidad (Localidad localidad)
        {
            if (localidad == null) throw new ArgumentNullException("localidad");

            _localidades.Add(localidad);

            localidad.Provincia = this;

            return this;
        }
    }
}