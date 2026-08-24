using System;
using System.Collections.Generic;
using System.Text;
using LifeCycle.DataAccess.Repository.IRepository;

namespace LifeCycle.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public T Get(int id)
        {
            throw new NotImplementedException();
        }

        public void Add(T entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(T entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }
    }
}
