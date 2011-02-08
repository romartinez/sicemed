﻿using System.Web.Mvc;
using Sicemed.Web.Models;

namespace Sicemed.Web.ModelBinders {
    public class ComplexObjectModelBinder : DefaultModelBinder{
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            if (typeof(ViewModelBase).IsAssignableFrom(bindingContext.ModelMetadata.ModelType)) {
                var serialized =
                    controllerContext.RequestContext.HttpContext.Request.Params[bindingContext.ModelName];
                if (string.IsNullOrWhiteSpace(serialized)) return null;

                var deserializeMethod = typeof(UrlHelperExtensions).GetMethod("Deserialize").MakeGenericMethod(bindingContext.ModelMetadata.ModelType);
                var completedEntity = deserializeMethod.Invoke(null, new object[] { serialized });
                return completedEntity;
            }
            return base.BindModel(controllerContext, bindingContext);
        }
    }
}