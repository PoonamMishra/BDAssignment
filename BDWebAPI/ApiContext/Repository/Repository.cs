using BDWebAPI.Models.Entities;
using BDWebAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BDWebAPI.ApiContext.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected RepositoryContext RepositoryContext { get; set; }
        private readonly ILogger<Repository<T>> _logger;


        public Repository(RepositoryContext repositoryContext, ILogger<Repository<T>> logger)
        {
            this.RepositoryContext = repositoryContext;
            this._logger = logger;

        }


        public IQueryable<T> FindAll()
        {
            return this.RepositoryContext.Set<T>().AsNoTracking();
        }

        public async Task<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return await this.RepositoryContext.Set<T>().Where(expression).FirstOrDefaultAsync();
        }

        //public void Create(T entity)
        //{
        //    this.RepositoryContext.Set<T>().Add(entity);
        //}

        public async Task Create(T entity)
        {
            await this.RepositoryContext.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            this.RepositoryContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            this.RepositoryContext.Set<T>().Remove(entity);
        }

        public async Task Save()
        {
            await this.RepositoryContext.SaveChangesAsync();
        }

        public void SaveBatch(Batch batch, EntityState entityState)
        {
            using var batchContext = new BatchContext();
            batchContext.Entry(batch).State = entityState;

            _logger.LogDebug("Start SaveBatch...");

            int x = (batchContext.SaveChanges());

            _logger.LogDebug("Finished SaveBatch...");
        }

        public Batch GetBatches(int batchId, int groupId)
        {
            Batch batch = null;

            using (var batchContext = new BatchContext())
            {
                var x = batchContext.Batches;
                batch = batchContext.Batches.Where(batc => batc.BatchId.Equals(batchId) && batc.GroupId.Equals(groupId)).FirstOrDefault();

                _logger.LogDebug("Finished Getting Batch...");
            }

            return batch;
        }
    }
}
