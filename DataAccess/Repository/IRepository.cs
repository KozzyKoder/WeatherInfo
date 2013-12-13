using System;
using DataAccess.Entities;

namespace DataAccess.Repository
{
    public interface IRepository<T> where T : Entity
    {
        T Get(Guid id);
        void Save(T entity);
        void Update(T entity);
    }
}