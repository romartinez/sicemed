using System.Collections.Generic;

namespace Sicemed.Web.Infrastructure.Reports
{
    public interface IReport{}

    public interface IReport<out T> : IReport
    {
        IEnumerable<T> Execute();     
    }
}