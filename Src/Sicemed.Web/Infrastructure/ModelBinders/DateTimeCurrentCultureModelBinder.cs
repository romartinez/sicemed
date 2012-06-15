using System;
using System.Globalization;
using System.Web.Mvc;

namespace Sicemed.Web.Infrastructure.ModelBinders
{
    public class DateTimeCurrentCultureModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (controllerContext.RequestContext.HttpContext.Request.HttpMethod == "GET")
            {
                var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
                var modelState = new ModelState { Value = valueResult };
                object actualValue = null;
                try
                {
                    actualValue = Convert.ToDateTime(valueResult.AttemptedValue, CultureInfo.CurrentCulture);
                }
                catch (FormatException e)
                {
                    modelState.Errors.Add(e);
                }

                bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
                return actualValue;
            }
            return base.BindModel(controllerContext, bindingContext);
        }
    }
}