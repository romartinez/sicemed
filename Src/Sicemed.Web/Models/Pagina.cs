using System;
using System.ComponentModel.DataAnnotations;
using Iesi.Collections.Generic;

namespace Sicemed.Web.Models
{
    public class Pagina : Entity
    {
        private readonly ISet<Pagina> _hijos;

        public Pagina()
        {
            _hijos = new HashedSet<Pagina>();
        }

        [Required]
        public virtual string Nombre { get; set; }

        [Required]
        public virtual string Contenido { get; set; }

        [Required]
        public virtual string Url { get; set; }

        [Required]
        public virtual int Orden { get; set; }

        public virtual Pagina Padre { get; set; }

        public virtual ISet<Pagina> Hijos
        {
            get { return _hijos; }
        }

        public virtual Pagina AgregarHijo(Pagina pagina)
        {
            if (pagina == null) throw new ArgumentNullException("pagina");

            _hijos.Add(pagina);
            pagina.Padre = this;

            return this;
        }
    }
}