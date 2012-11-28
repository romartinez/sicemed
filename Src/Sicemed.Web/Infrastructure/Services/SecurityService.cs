using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Services
{
    public interface ISecurityService
    {
        void Validate(Persona user, ActionDescriptor actionDescriptor);
        bool Can(Persona user, ActionDescriptor actionDescriptor);
    }

    public class SecurityService : ISecurityService
    {
        #region ISecurityService Members

        public virtual void Validate(Persona user, ActionDescriptor actionDescriptor)
        {
            if (user == null) throw new SecurityException("The user is not Authenticated.");
            if (actionDescriptor == null) throw new ArgumentNullException("actionDescriptor");

            if (!Can(user, actionDescriptor))
                throw new SecurityException(
                    string.Format("The User '{0}' doesn't have the permissions to execute the '{1}/{2}' action.", user,
								  actionDescriptor.ControllerDescriptor.ControllerName, actionDescriptor.ActionName));
        }

        public virtual bool Can(Persona user, ActionDescriptor actionDescriptor)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (actionDescriptor == null) throw new ArgumentNullException("actionDescriptor");
//RM: Se agrega al perfil del profesional acceso a las funciones EDITAR PACIENTE y Otorgar Turno de la secretaria. La variable rta es para dar soporte a esto
            bool rta;
            var controllerAttributes =
                actionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof (AuthorizeItAttribute), true).Cast
                    <AuthorizeItAttribute>();
            var methodAttributes =
                actionDescriptor.GetCustomAttributes(typeof (AuthorizeItAttribute), true).Cast<AuthorizeItAttribute>();

            var attributes = new List<AuthorizeItAttribute>();
            attributes.AddRange(controllerAttributes);
            attributes.AddRange(methodAttributes);

            if (!attributes.Any())
                throw new SecurityException(
                    string.Format("The action '{0}/{1}' doesn't have permissions defined. " +
                    "By default the methods are secured, you must be explicit on permissions.",actionDescriptor.ControllerDescriptor.ControllerName, actionDescriptor.ActionName));

//RM: Se agrega al perfil del profesional acceso a las funciones EDITAR PACIENTE y Otorgar Turno de la secretaria
            if (user.IsInRole("Profesional") && (actionDescriptor.ActionName.Equals("EdicionPaciente") || actionDescriptor.ActionName.Equals("GetTurnosProfesional") || actionDescriptor.ActionName.Equals("GetEspecialidadesProfesional") || actionDescriptor.ActionName.Equals("OtorgarTurno")))
                rta = true;
            else
                rta= attributes.Any(attr => user.IsInRole(attr.Rol));

            return rta;
        }

        #endregion
    }
}