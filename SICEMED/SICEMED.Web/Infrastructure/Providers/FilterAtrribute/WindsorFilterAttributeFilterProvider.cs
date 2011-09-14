using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web.Mvc;
using Castle.Windsor;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Helpers;

namespace Sicemed.Web.Infrastructure.Providers.FilterAtrribute
{
    /// <summary>
    /// http://www.cprieto.com/index.php/2010/07/30/windsor-service-locator-for-asp-net-mvc3-preview-1/
    /// </summary>
    public class WindsorFilterAttributeFilterProvider : FilterAttributeFilterProvider
    {
        private readonly IWindsorContainer _container;

        private readonly IEnumerable<FilterAttribute> _globalFilters = new FilterAttribute[]
        {
            new AuditAtrribute(),
            new HandleErrorAttribute(),
            new MenuAttribute(),
            new HandleErrorAttribute(){ExceptionType = typeof(SecurityException), View = "PermissionError"},
        };

        public WindsorFilterAttributeFilterProvider(IWindsorContainer container)
        {
            _container = container;
        }

        protected override IEnumerable<FilterAttribute> GetControllerAttributes(
            ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var attributes = base.GetControllerAttributes(controllerContext, actionDescriptor);
            foreach(var attribute in attributes)
            {
                _container.BuildUp(attribute.GetType(), attribute);
            }

            return attributes;
        }

        protected override IEnumerable<FilterAttribute> GetActionAttributes(
            ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var attributes = new List<FilterAttribute>();
            attributes.AddRange(base.GetControllerAttributes(controllerContext, actionDescriptor));
            attributes.AddRange(base.GetActionAttributes(controllerContext, actionDescriptor));

            attributes.AddRange(_globalFilters);

            //By default add the AuthorizeItAttribute if is not present and not is anonymous
            //var attributesTypes = attributes.Select(x => x.GetType());
            //if(!attributesTypes.Contains(typeof(AnonymousAttribute))
            //    && !attributesTypes.Contains(typeof(AuthorizeItAttribute)))
            //    attributes.Add(new AuthorizeItAttribute());

            foreach(var attribute in attributes)
            {
                _container.BuildUp(attribute.GetType(), attribute);
            }

            return attributes;
        }
    }
}