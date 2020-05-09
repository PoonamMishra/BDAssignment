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
            int generatedNumber = -1;

            await Task.Run(() =>
            {

                for (int i = 1; i <= totalNumberToGenerate; i++)
                {
                    generatedNumber = GenerateNumber();
                    Task.Delay(5000).Wait();


                    ProcessorEventArgs generatorEventArgs = new ProcessorEventArgs
                    {
                        BatchId = batchId,
                        ComputedNumber = generatedNumber
                    };
                    OnNumberGeneration(generatorEventArgs);
                     //delay for 5 sec
                 }
            });

        }

        void OnNumberGeneration(ProcessorEventArgs generatorEventArgs)
        {
            GeneratorEventHandler?.Invoke(this, generatorEventArgs);
        }


        private int GenerateNumber()
        {
            Random randObj = new Random();
            return randObj.Next(1, 10);
        }
    }
}
