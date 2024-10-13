using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using API.Domain.Repositories;
using API.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Repositories
{
    public class RepositoryBase<T>: IRepositoryBase<T> where T : class
    {
        private readonly DataContext dataContext;

        public RepositoryBase(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        public void Add(T entity)
            => dataContext.Set<T>().Add(entity);
        
        public void Update(T entity)
            => dataContext.Set<T>().Update(entity);
        
        public void Delete(T entity)
            => dataContext.Set<T>().Remove(entity);
        
        public IQueryable<T> FindAll() => dataContext.Set<T>().AsNoTracking();
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
            => dataContext.Set<T>().AsNoTracking().Where(expression);
    }
}