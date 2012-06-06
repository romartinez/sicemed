using System.Web.Mvc;

namespace Sicemed.Web.Infrastructure.ModelBinders
{
    public class CustomBindeablePropertiesModelBinder : DefaultModelBinder
    {
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor)
        {
            var viewModel = bindingContext.Model as ICustomBindeableProperties;
            if (viewModel.SkipProperty(propertyDescriptor))
            {
                return;
            }
            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }
    }
}