using BDWebAPI.ApiContext.Repository;
using BDWebAPI.Models;
using BDWebAPI.Models.Entities;
using BDWebAPI.Worker;
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



        public IEnumerable<Batch> GetCurrentState()
        {
            return _batchRepository.FindAll();

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

        public string PerformeCalculation(BatchInput input)
        {
            ItemsPerBatch = input.BatchCount;

            for (int batchNo = 1; batchNo <= input.BatchCount; batchNo++)
            {
                _generatorManager.Generate(batchNo, input.ItemPerBatch);

            }

            return string.Empty;
        }

        public void GeneratorCallback(object sender, ProcessorEventArgs args)
        {

            string data = "nuber generated";
            _multiplierManager.Multiplier(args.BatchId, args.ComputedNumber);
        }

        public void MultiplierCallback(object sender, ProcessorEventArgs args)
        {

            string data = "nuber generated";
            int abc1 = args.BatchId;
            int abc2 = args.ComputedNumber;

            Batch existingBatch = batchOutput.BatchList.Where(x => x.BatchId.Equals(args.BatchId)).FirstOrDefault();
            if (existingBatch != null)
            {
                existingBatch.Total = existingBatch.Total + args.ComputedNumber;
                existingBatch.TotalRemainingItem = --existingBatch.TotalRemainingItem;
                existingBatch.TotalProcessedItem = ++existingBatch.TotalProcessedItem;
                _batchRepository.Update(existingBatch);
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
                batchOutput.BatchList.Add(batch);
                _batchRepository.Create(batch);


            }

            _batchRepository.Save();

        }


    }
}
