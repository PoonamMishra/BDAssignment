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
        public delegate Task MultiplierEventHandler(object sender, ProcessorEventArgs generatorEventArgs);

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

        public async Task PerformeCalculation(BatchInput input)
        {
            ItemsPerBatch = input.BatchCount;

            for (int batchNo = 1; batchNo <= input.BatchCount; batchNo++)
            {
                await Task.Run(() =>
                {
                    int id = Task.CurrentId.Value;
                    return _generatorManager.Generate(batchNo, input.ItemPerBatch);
                });

            }


        }

        public void GeneratorCallback(object sender, ProcessorEventArgs args)
        {

            string data = "nuber generated";
            _multiplierManager.Multiplier(args.BatchId, args.ComputedNumber);
        }

        public async Task MultiplierCallback(object sender, ProcessorEventArgs args)
        {

            await Task.Run(() =>
             {
                 int abc1 = args.BatchId;
                 int abc2 = args.ComputedNumber;

                 Batch existingBatch =  _batchRepository.FindByCondition(x => x.BatchId.Equals(args.BatchId)).Result;
                 if (existingBatch != null)
                 {
                     existingBatch.Total = existingBatch.Total + args.ComputedNumber;
                     existingBatch.TotalRemainingItem = --existingBatch.TotalRemainingItem;
                     existingBatch.TotalProcessedItem = ++existingBatch.TotalProcessedItem;
                     //_batchRepository.Update(existingBatch);
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


             });


        }
    }
}
