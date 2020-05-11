using BDWebAPI.Models;
using BDWebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;


namespace BDWebAPI.Worker
{
    public class GeneratorManager : IGeneratorManager
    {
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

        private Task GetGenerateNumberTask(int batchId)
        {
            return Task.Run(() =>
             {
                 int generatedNumber = GenerateNumber();
                 Task.Delay(5000).Wait();

                 ProcessorEventArgs generatorEventArgs = new ProcessorEventArgs
                 {
                     BatchId = batchId,
                     ComputedNumber = generatedNumber
                 };
                 OnNumberGeneration(generatorEventArgs);

             });

        }

        void OnNumberGeneration(ProcessorEventArgs generatorEventArgs)
        {
            GeneratorEventHandler?.Invoke(this, generatorEventArgs);
        }


        private int GenerateNumber()
        {
            Random randObj = new Random();
            return randObj.Next(1, 101);
        }
    }
}
