using System;
using DataAccess.Entities;

namespace DataAccess.Repository
{
    public interface IRepository<T> where T : Entity
    {
        T Get(Guid id);
        T Get(Func<T, bool> predicate);
        T Save(T entity);
        void Update(T entity);
    }
}