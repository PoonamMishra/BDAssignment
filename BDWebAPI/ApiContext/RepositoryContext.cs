using BDWebAPI.Models;
using BDWebAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
