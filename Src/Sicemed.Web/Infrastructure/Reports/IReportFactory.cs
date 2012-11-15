namespace Sicemed.Web.Infrastructure.Reports
{
    public interface IReportFactory
    {
        T Create<T>();
    }
}