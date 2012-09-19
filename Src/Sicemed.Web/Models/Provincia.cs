using System;
using Iesi.Collections.Generic;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;

namespace Sicemed.Web.Models
{
    public class Provincia : Entity
    {
        public Provincia()
        {
            _localidades = new HashedSet<Localidad>();
        }

        #region Primitive Properties

        [Requerido]
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