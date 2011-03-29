using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sicemed.Web.Models.Roles
{
    public class Profesional : Rol
    {
        public virtual string Matricula { get; set; }

        public virtual ISet<EspecialidadProfesional> EspecialidadProfesional { get; set; }
    }
}