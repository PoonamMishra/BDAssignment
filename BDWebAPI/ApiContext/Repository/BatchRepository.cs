using BDWebAPI.Models.Entities;

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
