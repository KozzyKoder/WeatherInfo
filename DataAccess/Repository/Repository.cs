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
            var session = Factory.OpenSession();

            var entity = session.Query<T>().FirstOrDefault(p => p.Id == id);

            session.Close();

            return entity;
        }

        public T Get(Func<T, bool> predicate)
        {
            var session = Factory.OpenSession();

            var entity = session.Query<T>().FirstOrDefault(predicate);

            session.Close();

            return entity;
        }

        public T Save(T entity)
        {
            var session = Factory.OpenSession();

            var id = (Guid)session.Save(entity);
            
            entity.Id = id;

            session.Flush();
            session.Close();

            return entity;
        }

        public void Update(T entity)
        {
            var session = Factory.OpenSession();

            session.Update(entity);

            session.Flush();
            session.Close();
        }
    }
}