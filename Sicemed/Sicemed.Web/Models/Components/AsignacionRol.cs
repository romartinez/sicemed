using System;

namespace Sicemed.Web.Models.Components
{
    public class AsignacionRol : ComponentBase
    {
        private readonly DateTime _fechaAsignacion;
        private readonly Rol _rol;
        
        public AsignacionRol(){}

        public AsignacionRol(Rol rol)
        {
            _fechaAsignacion = DateTime.Now;
            _rol = rol;
        }

        public virtual Rol Rol
        {
            get { return _rol; }
        }

        public virtual DateTime FechaAsignacion
        {
            get { return _fechaAsignacion; }            
        }
    }
}