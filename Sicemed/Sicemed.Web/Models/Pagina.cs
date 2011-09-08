using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sicemed.Web.Models
{
    public class Pagina : Entity
    {
        private ISet<Pagina> _hijos;

        public Pagina()
        {
            _hijos = new HashSet<Pagina>();
        }

        [Required]
        public virtual string Nombre { get; set; }
        [Required]
        public virtual string Contenido { get; set; }

        public virtual Pagina Padre { get; set; }

        public virtual IEnumerable<Pagina> Hijos
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