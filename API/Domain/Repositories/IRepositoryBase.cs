using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace API.Domain.Repositories
{
    public interface IRepositoryBase<T>
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
    }
}