using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BDWebAPI.ApiContext.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected RepositoryContext RepositoryContext { get; set; }

        public Repository(RepositoryContext repositoryContext)
        {
            this.RepositoryContext = repositoryContext;
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
    }
}
