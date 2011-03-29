using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sicemed.Web.Models.Roles
{
    public class Paciente : Rol
    {
        public virtual Plan Plan { get; set; }
    }
}