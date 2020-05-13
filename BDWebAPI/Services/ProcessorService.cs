using BDWebAPI.ApiContext.Repository;
using BDWebAPI.Models;
using BDWebAPI.Models.Entities;
using BDWebAPI.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDWebAPI.Services
{
    public class ProcessorService : IProcessorService
    {

        private readonly IGeneratorManager _generatorManager;
        private readonly IMultiplierManager _multiplierManager;
        private readonly IBatchRepository _batchRepository;
        private readonly ILogger<ProcessorService> _logger;

        public delegate void GeneratorEventHandler(object sender, ProcessorEventArgs generatorEventArgs);
        public delegate void MultiplierEventHandler(object sender, ProcessorEventArgs generatorEventArgs);


        static readonly object pblock = new object();

        int ItemsPerBatch = 0;

        public static int GroupId { get; set; } = 0;
        public static bool IsProcessCompleted { get; set; } = false;

        public ProcessorService(IGeneratorManager generatorManager,
            IMultiplierManager multiplierManager,
            IBatchRepository batchRepository,
            ILogger<ProcessorService> logger)
        {

            _generatorManager = generatorManager;
            _multiplierManager = multiplierManager;
            _batchRepository = batchRepository;
            _logger = logger;

            _generatorManager.GeneratorEventHandler += GeneratorCallback;
            _multiplierManager.MultiplierEventHandler += MultiplierCallback;
        }



        public async Task<IEnumerable<Batch>> GetCurrentState(int? groupId = null)
        {
            groupId = groupId == null ? GroupId : groupId;

            using var batchContext = new BatchContext();
            return await batchContext.Batches.Where(batch => batch.GroupId.Equals(groupId)).OrderBy(ord => ord.BatchId).ToListAsync();

        }

        public async Task<IEnumerable<Batch>> GetAllBatches()
        {

            using var batchContext = new BatchContext();
            return await batchContext.Batches.OrderBy(ord => ord.BatchId).ToListAsync();

        }


        public async Task<IEnumerable<Batch>> GetPreviousBatch()
        {
            return await GetCurrentState(GroupId - 1);
        }

        public async Task PerformeCalculation(BatchInput input)
        {
            ItemsPerBatch = input.ItemsPerBatch;
            IsProcessCompleted = false;

            IEnumerable<int> integerList = Enumerable.Range(1, input.BatchSize).ToList();
            GroupId++;
            var myTask = new List<Task>();

            Parallel.ForEach(integerList, i =>
                {
                    myTask.Add(_generatorManager.Generate(i, input.ItemsPerBatch));
                });

            await Task.WhenAll(myTask).ContinueWith(res => IsProcessCompleted = true);

        }

        public void GeneratorCallback(object sender, ProcessorEventArgs args)
        {

            _multiplierManager.Multiplier(args.BatchId, args.ComputedNumber);
        }

        public void MultiplierCallback(object sender, ProcessorEventArgs args)
        {
            lock (pblock)
            {

                var dbBatch = GetBatches(args.BatchId);


                if (dbBatch != null)
                {

                    dbBatch.Total += args.ComputedNumber;
                    dbBatch.TotalRemainingItem = --dbBatch.TotalRemainingItem;
                    dbBatch.TotalProcessedItem = ++dbBatch.TotalProcessedItem;
                    SaveBatch(dbBatch, EntityState.Modified);
                }
                else
                {

                    Batch batch = new Batch()
                    {
                        GroupId = GroupId,
                        BatchId = args.BatchId,
                        Total = args.ComputedNumber,
                        TotalRemainingItem = ItemsPerBatch - 1,
                        TotalProcessedItem = 1
                    };
                    SaveBatch(batch, EntityState.Added);
                }

            }

        }

        private Batch GetBatches(int batchId)
        {
            Batch batch = null;

            using (var batchContext = new BatchContext())
            {
                var x = batchContext.Batches;
                batch = batchContext.Batches.Where(batc => batc.BatchId.Equals(batchId) && batc.GroupId.Equals(GroupId)).FirstOrDefault();

                _logger.LogDebug("Finished Getting Batch...");
            }

            return batch;
        }

        private void SaveBatch(Batch batch, EntityState entityState)
        {
            using var batchContext = new BatchContext();
            batchContext.Entry(batch).State = entityState;

            _logger.LogDebug("Start SaveBatch...");


            int x = (batchContext.SaveChanges());

            _logger.LogDebug("Finished SaveBatch...");
        }
    }
}
