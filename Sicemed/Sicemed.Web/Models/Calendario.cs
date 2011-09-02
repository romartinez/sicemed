using System;
using System.Collections.Generic;


namespace Sicemed.Web.Models
{
    public class Calendario : Entity
    {
        #region Primitive Properties

        public virtual DateTime Anio { get; set; }

        public virtual string Nombre { get; set; }

        #endregion

        #region Navigation Properties

        private ISet<Feriado> _feriados;
        public virtual IEnumerable<Feriado> Feriados
        {
            get { return _feriados; }
            protected internal set { _feriados = new HashSet<Feriado>(value); }
        }
        #endregion

        #region Ctos
        public Calendario()
        {
            _feriados = new HashSet<Feriado>();
        }
        #endregion

        #region Methods
        public virtual Calendario AddFeriado(Feriado feriado)
        {
            if(_feriados.Contains(feriado)) return this;
            _feriados.Add(feriado);
            feriado.Calendario = this;
            return this;
        }
        #endregion
    }
}