using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BDWebAPI.Services.ProcessorService;

namespace BDWebAPI.Worker
{
    public interface IGeneratorManager
    {
        event GeneratorEventHandler GeneratorEventHandler;

        void Generate(int batchId, int totalNumberToGenerate);


    }
}
