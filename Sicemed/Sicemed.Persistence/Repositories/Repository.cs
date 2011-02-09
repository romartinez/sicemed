using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Linq;
using Sicemed.Model;

namespace Sicemed.Persistence.Repositories {
    public class Repository<T> : IRepository<T> 
        where T : EntityBase {
        private readonly ISession _session;

        public Repository(ISession session) {
            _session = session;
        }

        public Type ElementType {
            get { return _session.Query<T>().ElementType; }
        }

        public Expression Expression {
            get { return _session.Query<T>().Expression; }
        }

        public IQueryProvider Provider {
            get { return _session.Query<T>().Provider; }
        }

        public void Add(T entity) {
            _session.Save(entity);
        }

        public T Get(int id) {
            return _session.Get<T>(id);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator() {
            return _session.Query<T>().GetEnumerator();
        }

        public void Remove(T entity) {
            _session.Delete(entity);
        }   
    }
}
