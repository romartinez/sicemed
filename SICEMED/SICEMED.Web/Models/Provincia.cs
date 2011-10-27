using System;
using System.ComponentModel.DataAnnotations;
using Iesi.Collections.Generic;

namespace Sicemed.Web.Models
{
    public class Provincia : Entity
    {
        public Provincia()
        {
            _localidades = new HashedSet<Localidad>();
        }

        #region Primitive Properties

        [Required]
        public virtual string Nombre { get; set; }

        #endregion

        #region Navigation Properties

        private readonly ISet<Localidad> _localidades;

        public virtual ISet<Localidad> Localidades
        {
            get { return _localidades; }
        }

        #endregion

        public virtual Provincia AgregarLocalidad(Localidad localidad)
        {
            if (localidad == null) throw new ArgumentNullException("localidad");

            _localidades.Add(localidad);

            localidad.Provincia = this;

            return this;
        }
    }
}