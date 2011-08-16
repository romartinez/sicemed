using System.Reflection;
using System.Web.Mvc;
using Sicemed.Web.Models;
using Sicemed.Web.Models.ViewModels;
using Sicemed.Web.Plumbing.Helpers;

namespace Sicemed.Web.Plumbing.ModelBinders
{
    public class ComplexObjectModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (typeof (ViewModelBase).IsAssignableFrom(bindingContext.ModelMetadata.ModelType))
            {
                string serialized =
                    controllerContext.RequestContext.HttpContext.Request.Params[bindingContext.ModelName];
                if (string.IsNullOrWhiteSpace(serialized)) return null;

                MethodInfo deserializeMethod =
                    typeof (UrlHelperExtensions).GetMethod("Deserialize").MakeGenericMethod(
                        bindingContext.ModelMetadata.ModelType);
                object completedEntity = deserializeMethod.Invoke(null, new object[] {serialized});
                return completedEntity;
            }
            return base.BindModel(controllerContext, bindingContext);
        }
    }
}