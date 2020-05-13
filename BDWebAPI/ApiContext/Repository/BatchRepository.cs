using BDWebAPI.Models.Entities;
using Microsoft.Extensions.Logging;

namespace BDWebAPI.ApiContext.Repository
{
    public class BatchRepository : Repository<Batch>, IBatchRepository
    {

        //private readonly ILogger<BatchRepository> _logger;

        public BatchRepository(RepositoryContext repositoryContext, ILogger<BatchRepository> logger)
    : base(repositoryContext,logger)
        {
            

        }



    }
}
