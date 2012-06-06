using System;
using System.Web.Mvc;
using Sicemed.Web.Areas.Admin.Models.Personas;

namespace Sicemed.Web.Infrastructure.ModelBinders
{
    public class PersonaEditModelBinder : DefaultModelBinder
    {
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor)
        {
            var viewModel = bindingContext.Model as PersonaEditModel;
            if (SkipProperty(viewModel, propertyDescriptor.PropertyType))
            {
                return;
            }
            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }

        protected virtual bool SkipProperty(PersonaEditModel viewModel, Type propertyType)
        {
            return (!viewModel.EsPaciente && propertyType == typeof (PacienteEditModel))
                || (!viewModel.EsSecretaria && propertyType == typeof(SecretariaEditModel))
                || (!viewModel.EsProfesional && propertyType == typeof(ProfesionalEditModel));
        }
    }
}