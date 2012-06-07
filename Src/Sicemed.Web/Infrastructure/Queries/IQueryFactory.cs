namespace Sicemed.Web.Infrastructure.Queries
{
    public interface IQueryFactory
    {
        T Create<T>();
    }
}