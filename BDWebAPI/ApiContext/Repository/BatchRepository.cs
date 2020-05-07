using BDWebAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDWebAPI.ApiContext.Repository
{
    public class BatchRepository : Repository<Batch>, IBatchRepository
    {
        public BatchRepository(RepositoryContext repositoryContext)
    : base(repositoryContext)
        {

        }



    }
}
