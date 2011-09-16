using System;
using System.Linq;
using Castle.Windsor;

namespace Sicemed.Web.Infrastructure.Helpers
{
    public static class WindsorExtensions
    {
        public static void BuildUp(this IWindsorContainer container, Type type, object instance)
        {
            var properties = type.GetProperties().Where(p => p.CanWrite && p.PropertyType.IsPublic);
            foreach (var propertyInfo in properties)
            {
                if (container.Kernel.HasComponent(propertyInfo.PropertyType))
                {
                    propertyInfo.SetValue(instance, container.Resolve(propertyInfo.PropertyType), null);
                }
            }
        }
    }
}