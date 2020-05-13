using BDWebAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BDWebAPI.ApiContext
{
    public class RepositoryContext :DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) :base(options){ 
        
        }

        public RepositoryContext() 
        {

        }

        public DbSet<Batch> Batches { get; set; }
       
    }
}
