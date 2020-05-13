using BDWebAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BDWebAPI.ApiContext.Repository
{

    public class BatchContext : DbContext
    {
        public static IConfigurationRoot Configuration { get; set; }

        
        public DbSet<Batch> Batches { get; set; }
        //public EmailContext(DbContextOptions<EmailContext> options)
        //    : base(options)
        //{ }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            optionsBuilder.UseInMemoryDatabase("BDAssessment");
        }


    }
}
