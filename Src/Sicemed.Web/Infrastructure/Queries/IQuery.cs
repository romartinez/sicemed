namespace Sicemed.Web.Infrastructure.Queries
{
    public interface IQuery
    {
        void ClearCache();
    }
    
    public interface IQuery<T> : IQuery
    {
        T Execute();
    }
}