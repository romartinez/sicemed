using System.Collections.Generic;

namespace Sicemed.Web.Infrastructure.Reports
{
    public interface IReport
    {
        string Title { get; }
        string Name { get; }
        string DataSource { get; }
        Dictionary<string, string> Parameters { get; }        
    }
    
    public interface IReport<out T> : IReport
    {
        IEnumerable<T> Execute();
    }
}