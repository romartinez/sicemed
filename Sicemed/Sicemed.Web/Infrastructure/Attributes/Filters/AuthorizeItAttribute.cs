using System;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Services;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Infrastructure.Attributes.Filters
{
    public class AuthorizeItAttribute : AuthorizeAttribute
    {
        private readonly Type _rolType;

        public AuthorizeItAttribute(Type rolType)
        {
            if (rolType == null) throw new ArgumentNullException("rolType");
            if (rolType.IsAssignableFrom(typeof (Rol)))
                throw new ArgumentException(@"El tipo debe ser un Rol.", "rolType");

            _rolType = rolType;
        }

        public virtual IMembershipService MembershipService { get; set; }
        public virtual ISecurityService SecurityService { get; set; }

        public virtual Type Rol
        {
            get { return _rolType; }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //Check is logged on
            base.OnAuthorization(filterContext);

            if (!base.AuthorizeCore(filterContext.HttpContext)) return;

            //Authorize
            var currentUser = MembershipService.GetCurrentUser();

            SecurityService.Validate(currentUser, filterContext.ActionDescriptor);
        }
    }
}