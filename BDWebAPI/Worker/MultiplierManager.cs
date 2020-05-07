using BDWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BDWebAPI.Services.ProcessorService;

namespace BDWebAPI.Worker
{
    public class MultiplierManager :IMultiplierManager
    {
        public event MultiplierEventHandler MultiplierEventHandler;
        public void Multiplier(int batchId, int number)
        {
            Random randObj = new Random();
            int randomNo= randObj.Next(2, 4);
            ProcessorEventArgs ProcessorEventArgs = new ProcessorEventArgs();

            ProcessorEventArgs.BatchId = batchId;
            ProcessorEventArgs.ComputedNumber = randomNo * number;
            OnNumberGeneration(ProcessorEventArgs);
        }

        
        void OnNumberGeneration(ProcessorEventArgs generatorEventArgs)
        {
            MultiplierEventHandler?.Invoke(this, generatorEventArgs);
        }



    }
}
