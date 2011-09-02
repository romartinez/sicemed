using System;
using System.Web.Mvc;
using System.Web.Services.Description;
using Sicemed.Web.Infrastructure.Services;

namespace Sicemed.Web.Infrastructure.Attributes.Filters
{
    public class AuthorizeItAttribute : AuthorizeAttribute
    {
        private readonly Type _actionType;

        public AuthorizeItAttribute(){}

        public AuthorizeItAttribute(Type actionType)
        {
            if (actionType == null) throw new ArgumentNullException("actionType");
            if(!typeof(Operation).IsAssignableFrom(actionType)) throw new ArgumentNullException("actionType", "actionType must be of the type Action.");
            _actionType = actionType;
        }

        public virtual IMembershipService MembershipService { get; set; }
        public virtual ISecurityService SecurityService { get; set; }

        public virtual Type ActionType
        {
            get { return _actionType; }
        }

        public virtual bool IsActionSpecific
        {
            get { return _actionType != null; }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //Check is logged on
            base.OnAuthorization(filterContext);

            if(_actionType == null || !base.AuthorizeCore(filterContext.HttpContext)) return;

            //Authorize
            var currentUser = MembershipService.GetCurrentUser();

            SecurityService.Validate(currentUser, filterContext.ActionDescriptor);
        }
    }
}