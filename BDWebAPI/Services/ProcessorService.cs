using BDWebAPI.ApiContext;
using BDWebAPI.ApiContext.Repository;
using BDWebAPI.Models;
using BDWebAPI.Models.Entities;
using BDWebAPI.Worker;
using Microsoft.EntityFrameworkCore;
using System;
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

        int ItemsPerBatch = 0;

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



        public async Task<IEnumerable<Batch>> GetCurrentState()
        {

            using (var batchContext = new BatchContext())
            {

                return await batchContext.Batches.ToListAsync();

            }

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

            List<int> integerList = Enumerable.Range(1, input.BatchSize).ToList();

            Parallel.ForEach(integerList, i =>
            {
                _generatorManager.Generate(i, input.ItemsPerBatch);
            });




        }

        public void GeneratorCallback(object sender, ProcessorEventArgs args)
        {

            _multiplierManager.Multiplier(args.BatchId, args.ComputedNumber);
        }

        public void MultiplierCallback(object sender, ProcessorEventArgs args)
        {



            int batchId = args.BatchId;



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
                    BatchId = args.BatchId,
                    Total = args.ComputedNumber,
                    TotalRemainingItem = ItemsPerBatch - 1,
                    TotalProcessedItem = 1
                };
                SaveBatch(batch, EntityState.Added);
            }






        }

        private Batch GetBatches(int batchId)
        {
            Batch batch = null;

            using (var batchContext = new BatchContext())
            {
                var x = batchContext.Batches;
                batch = batchContext.Batches.Where(batc => batc.BatchId.Equals(batchId)).FirstOrDefault();

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
