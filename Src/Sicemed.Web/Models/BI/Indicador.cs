using System;
using System.ComponentModel.DataAnnotations;
using Iesi.Collections.Generic;
using Sicemed.Web.Models.BI.Enumerations;

namespace Sicemed.Web.Models.BI
{
    public class Indicador : Entity
    {
        [Required]
        public virtual CategoriaIndicador Categoria { get; set; }
        [Required]
        public virtual string Nombre { get; set; }
        public virtual bool Habilitado { get; set; }
        public virtual string Descripcion { get; set; }
        [Required]
        public virtual TipoOperadorIndicador TipoOperador { get; set; }

        public virtual ISet<ObjetivoIndicador> Objetivos { get; set; }

        public Indicador ()
        {
            Objetivos = new HashedSet<ObjetivoIndicador>();
        }
    }
}