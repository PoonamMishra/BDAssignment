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
        public async Task Multiplier(int batchId, int number)
        {

            await Task.Run(() =>
            {
                Random randObj = new Random();
                int randomNo = randObj.Next(2, 4);
                Task.Delay(5000);
                ProcessorEventArgs ProcessorEventArgs = new ProcessorEventArgs();

                ProcessorEventArgs.BatchId = batchId;
                ProcessorEventArgs.ComputedNumber = randomNo * number;
                OnNumberGeneration(ProcessorEventArgs);

            });


        }


        void OnNumberGeneration(ProcessorEventArgs generatorEventArgs)
        {
            MultiplierEventHandler?.Invoke(this, generatorEventArgs);
        }



    }
}
