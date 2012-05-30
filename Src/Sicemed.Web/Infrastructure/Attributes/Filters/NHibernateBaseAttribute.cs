using System.Web.Mvc;
using NHibernate;

namespace Sicemed.Web.Infrastructure.Attributes.Filters
{
    public class NHibernateBaseAttribute : ActionFilterAttribute
    {
        public virtual ISessionFactory SessionFactory { get; set; }
    }
}