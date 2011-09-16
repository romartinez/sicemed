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
                    string.Format("The User '{0}' doesn't have the permissions to execute '{1}'.", user.Membership.Email,
                                  actionDescriptor));
        }

        public virtual bool Can(Persona user, ActionDescriptor actionDescriptor)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (actionDescriptor == null) throw new ArgumentNullException("actionDescriptor");

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
                    "El método no tiene definidos permisos. Por defecto los métodos son seguros, la seguridad debe ser explpícita.");

            return attributes.Any(attr => user.IsInRole(attr.Rol));
        }

        #endregion
    }
}