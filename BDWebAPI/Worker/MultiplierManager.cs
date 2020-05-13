using BDWebAPI.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;


namespace BDWebAPI.Worker
{
    public class MultiplierManager : IMultiplierManager
    {
        private readonly ILogger<MultiplierManager> _logger;

        public MultiplierManager(ILogger<MultiplierManager> logger)
        {
            _logger = logger;
        }

        public event Services.ProcessorService.MultiplierEventHandler MultiplierEventHandler;
        public void Multiplier(int batchId, int number)
        {
            int randNum = new Random().Next(2, 5);
           

            ProcessorEventArgs ProcessorEventArgs = new ProcessorEventArgs
            {
                BatchId = batchId,
                ComputedNumber = randNum * number,
                
            };

            _logger.LogInformation("Batch Id: =" + batchId + " GeneratedNo ="+ number +" Multiplier Number: =" + randNum +" Computed No= "+ randNum * number);

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
