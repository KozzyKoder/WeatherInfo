using System;
using System.Linq;
using DataAccess.Entities;
using NHibernate;
using NHibernate.Linq;

namespace DataAccess.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : Entity
    {
        protected ISessionFactory Factory;
        
        public T Get(Guid id)
        {
            var session = Factory.OpenSession();

            var entity = session.Query<T>().FirstOrDefault(p => p.Id == id);

            session.Close();

            return entity;
        }

        public void Save(T entity)
        {
            var session = Factory.OpenSession();

            session.Save(entity);

            session.Flush();
            session.Close();
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