using System.Linq;
using Sicemed.Model;

namespace Sicemed.Persistence.Repositories {
    public interface IRepository <T> : IQueryable<T> 
        where T: EntityBase{
        void Add(T entity);
        T Get(int id);
        void Remove(T entity);
    }
}