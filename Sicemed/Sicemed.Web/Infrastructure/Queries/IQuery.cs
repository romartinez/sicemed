using System.Collections.Generic;

namespace Sicemed.Web.Infrastructure.Queries
{
    public interface IQuery<T>
    {
        IEnumerable<T> Execute();
    }
}