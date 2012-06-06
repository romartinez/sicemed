using System.ComponentModel;

namespace Sicemed.Web.Infrastructure.ModelBinders
{
    public interface ICustomBindeableProperties
    {
        bool SkipProperty(PropertyDescriptor propertyDescriptor);
    }
}