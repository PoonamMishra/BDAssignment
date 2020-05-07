using BDWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BDWebAPI.Services.ProcessorService;

namespace BDWebAPI.Worker
{
    public class GeneratorManager : IGeneratorManager
    {
        public event GeneratorEventHandler GeneratorEventHandler;

        public void Generate(int batchId, int totalNumberToGenerate)
        {
            int generatedNumber = -1;

            for (int i = 1; i <= totalNumberToGenerate; i++)
            {
                generatedNumber = GenerateNumber();

                ProcessorEventArgs generatorEventArgs = new ProcessorEventArgs();
                generatorEventArgs.BatchId = batchId;
                generatorEventArgs.ComputedNumber = generatedNumber;
                OnNumberGeneration(generatorEventArgs);
                //delay for 5 sec
            }
            
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
