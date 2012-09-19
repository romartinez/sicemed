using System;
using System.ComponentModel.DataAnnotations;
using Iesi.Collections.Generic;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;
using Sicemed.Web.Models.BI.Enumerations;

namespace Sicemed.Web.Models.BI
{
    public class Indicador : Entity
    {
        [Requerido]
        public virtual CategoriaIndicador Categoria { get; set; }

        [Requerido]
        public virtual string Nombre { get; set; }
        public virtual bool Habilitado { get; set; }
        public virtual string Descripcion { get; set; }

        public virtual string NumeradorSql { get; set; }
        public virtual string DenominadorSql { get; set; }

        [Requerido]
        public virtual TipoOperadorIndicador TipoOperador { get; set; }

        private readonly ISet<ObjetivoIndicador> _objetivos;

        public virtual ISet<ObjetivoIndicador> Objetivos
        {
            get { return _objetivos; }
        }

        public Indicador()
        {
            _objetivos = new HashedSet<ObjetivoIndicador>();
        }

        public virtual void AgregarObjetivo(ObjetivoIndicador objetivo)
        {
            _objetivos.Add(objetivo);
        }

        public virtual void QuitarObjetivo(ObjetivoIndicador objetivo)
        {
            _objetivos.Remove(objetivo);
        }
    }
}