using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BDWebAPI.ApiContext.Repository
{
    public interface IRepository<T>
    {
        IQueryable<T> FindAll();
        Task<T> FindByCondition(Expression<Func<T, bool>> expression);
        Task Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task Save();
    }
}
