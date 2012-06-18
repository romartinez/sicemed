using System;
using System.Web.Mvc;

namespace Sicemed.Web.Infrastructure.ModelBinders
{
    /// <summary>
    /// NOTE: Workaround para poder usar search == null en los controllers.
    /// </summary>
    public class SearchFiltersModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var prefix = string.Empty;
            if (!String.IsNullOrEmpty(bindingContext.ModelName)
                && bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName))
                prefix = bindingContext.ModelName + ".";


            var valueProvided = bindingContext.ValueProvider.GetValue(string.Format("{0}Filtro", prefix));
            if (valueProvided == null) return null;

            return base.BindModel(controllerContext, bindingContext);
        }
    }
}