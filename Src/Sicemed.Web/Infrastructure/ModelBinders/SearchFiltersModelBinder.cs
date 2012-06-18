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
            //Si el formulario es posteado, la variable viene como string.Empty, si es NULL es porque no fue posteado
            if (bindingContext.ValueProvider.GetValue("Filtro") == null) return null;            

            return base.BindModel(controllerContext, bindingContext);
        }
    }
}