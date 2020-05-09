using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BDWebAPI.Services.ProcessorService;

namespace BDWebAPI.Worker
{
    public interface IMultiplierManager
    {
        event MultiplierEventHandler MultiplierEventHandler;

        Task Multiplier(int batchId, int number);

    }
}
