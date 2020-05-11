using BDWebAPI.ApiContext;
using BDWebAPI.ApiContext.Repository;
using BDWebAPI.Models;
using BDWebAPI.Models.Entities;
using BDWebAPI.Worker;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
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

        public delegate void GeneratorEventHandler(object sender, ProcessorEventArgs generatorEventArgs);
        public delegate void MultiplierEventHandler(object sender, ProcessorEventArgs generatorEventArgs);

        BatchOutput batchOutput = new BatchOutput();

        static readonly object pblock = new object();

        int ItemsPerBatch = 0;

        public static int GroupId { get; set; } = 0;
        public static bool IsProcessCompleted { get; set; } = false;

        public ProcessorService(IGeneratorManager generatorManager,
            IMultiplierManager multiplierManager,
            IBatchRepository batchRepository)
        {

            _generatorManager = generatorManager;
            _multiplierManager = multiplierManager;
            _batchRepository = batchRepository;

            _generatorManager.GeneratorEventHandler += GeneratorCallback;
            _multiplierManager.MultiplierEventHandler += MultiplierCallback;
        }



        public async Task<IEnumerable<Batch>> GetCurrentState(int? groupId = null)
        {
            groupId = groupId == null ? GroupId : groupId;

            using (var batchContext = new BatchContext())
            {

                return await batchContext.Batches.Where(batch => batch.GroupId.Equals(groupId)).ToListAsync();

            }

        }

        public async Task<IEnumerable<Batch>> GetAllBAtches()
        {
           
            using (var batchContext = new BatchContext())
            {

                return await batchContext.Batches.ToListAsync();

            }

        }


        public async Task<IEnumerable<Batch>> GetPreviousBatch()
        {
            return await GetCurrentState(GroupId - 1);
        }


        public BatchOutput GetCurrentState1()
        {
            Batch batch1 = new Batch() { BatchId = 1, Total = 5, TotalProcessedItem = 2, TotalRemainingItem = 3 };
            Batch batch2 = new Batch() { BatchId = 2, Total = 5, TotalProcessedItem = 2, TotalRemainingItem = 3 };
            Batch batch3 = new Batch() { BatchId = 3, Total = 5, TotalProcessedItem = 2, TotalRemainingItem = 3 };
            Batch batch4 = new Batch() { BatchId = 4, Total = 5, TotalProcessedItem = 2, TotalRemainingItem = 3 };

            List<Batch> batches = new List<Batch>();
            batches.Add(batch1);
            batches.Add(batch2);
            batches.Add(batch3);
            batches.Add(batch4);

            BatchOutput batchOutput = new BatchOutput()
            {
                GroupBatchId = 1,
                BatchList = batches,
                Total = 100
            };

            return batchOutput;
        }

        public string PerformeCalculation()
        {
            return "This will calcuate the result";
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

                    dbBatch.Total = dbBatch.Total + args.ComputedNumber;
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

                Console.WriteLine("Finished Getting Batch...");
            }

            return batch;
        }

        private void SaveBatch(Batch batch, EntityState entityState)
        {
            using (var batchContext = new BatchContext())
            {
                batchContext.Entry(batch).State = entityState;


                Console.WriteLine("Start SaveBatch...");

                int x = (batchContext.SaveChanges());

                Console.WriteLine("Finished SaveBatch...");
            }
        }
    }
}
