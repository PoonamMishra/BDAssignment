using BDWebAPI.Models;
using BDWebAPI.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BDWebAPI.Worker
{
    public class GeneratorManager : IGeneratorManager
    {
        private readonly ILogger<GeneratorManager> _logger;

        public GeneratorManager(ILogger<GeneratorManager> logger)
        {
            _logger = logger;
        }

        public event ProcessorService.GeneratorEventHandler GeneratorEventHandler;

        public async Task Generate(int batchId, int totalNumberToGenerate)
        {
           
            var myTask = new List<Task>();
            IEnumerable<int> integerList = Enumerable.Range(1, totalNumberToGenerate).ToList();

            Parallel.ForEach(integerList, i =>
            {
                myTask.Add(GetGenerateNumberTask(batchId));
            });

            await Task.WhenAll(myTask);
        }

        public Task GetGenerateNumberTask(int batchId)
        {
            return Task.Run(() =>
             {
                 int generatedNumber = GenerateNumber();

                 _logger.LogInformation("Batch Id: ="+ batchId + "Generated No: ="+ generatedNumber);

                 Task.Delay(5000).Wait();

                 ProcessorEventArgs generatorEventArgs = new ProcessorEventArgs
                 {
                     BatchId = batchId,
                     ComputedNumber = generatedNumber
                 };
                 OnNumberGeneration(generatorEventArgs);

             });

        }

        public void OnNumberGeneration(ProcessorEventArgs generatorEventArgs)
        {
            GeneratorEventHandler?.Invoke(this, generatorEventArgs);
        }


        public int GenerateNumber()
        {
            Random randObj = new Random();
            return randObj.Next(1, 101);
        }
    }
}
