using System;
using Iesi.Collections.Generic;

namespace Sicemed.Web.Models
{
    public class Calendario : EntityBase
    {
        #region Primitive Properties

        public virtual DateTime Anio { get; set; }

        public virtual string Nombre { get; set; }

        #endregion

        #region Navigation Properties

        private ISet<Feriado> _feriados;
        public virtual ISet<Feriado> Feriados
        {
            get { return new ImmutableSet<Feriado>(_feriados); }
            private set { _feriados = value; }
        }
        #endregion

        #region Ctos
        public Calendario()
        {
            _feriados = new HashedSet<Feriado>();
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