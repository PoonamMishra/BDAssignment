using BDWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BDWebAPI.Worker
{
    public class MultiplierManager : IMultiplierManager
    {
        public event Services.ProcessorService.MultiplierEventHandler MultiplierEventHandler;
        public void Multiplier(int batchId, int number)
        {
            int randNum = new Random().Next(2, 5);
            ProcessorEventArgs ProcessorEventArgs = new ProcessorEventArgs();

            ProcessorEventArgs.BatchId = batchId;
            ProcessorEventArgs.ComputedNumber = randNum * number;


            Task b = Task.Delay(5000);
            b.Wait();
            OnNumberGeneration(ProcessorEventArgs);           


        }


        void OnNumberGeneration(ProcessorEventArgs generatorEventArgs)
        {
            MultiplierEventHandler?.Invoke(this, generatorEventArgs);
        }



    }
}
