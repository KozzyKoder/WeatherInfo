using System;
using System.Linq;
using DataAccess.Entities;
using NHibernate;
using NHibernate.Linq;

namespace DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        protected ISessionFactory Factory;

        public Repository()
        {
            Factory = SqliteConfigurator.GetSessionFactory();
        }

        public T Get(Guid id)
        {
            T entity;
            using (var session = Factory.OpenSession())
            {
                entity = session.Query<T>().FirstOrDefault(p => p.Id == id);
            }

            return entity;
        }

        public T Get(Func<T, bool> predicate)
        {
            T entity;
            using (var session = Factory.OpenSession())
            {
                entity = session.Query<T>().FirstOrDefault(predicate);
            }

            return entity;
        }

        public T Save(T entity)
        {
            using (var session = Factory.OpenSession())
            {
                var id = (Guid)session.Save(entity);
            
                entity.Id = id;

                session.Flush();
            }

            return entity;
        }

        public void Update(T entity)
        {
            using (var session = Factory.OpenSession())
            {
                session.Update(entity);

                session.Flush();
            }
        }
    }
}