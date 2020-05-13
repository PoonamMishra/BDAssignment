using BDWebAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
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

        void SaveBatch(Batch batch, EntityState entityState);
        Batch GetBatches(int batchId, int groupId);
    }
}
