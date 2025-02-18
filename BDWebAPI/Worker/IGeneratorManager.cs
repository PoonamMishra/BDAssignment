﻿using BDWebAPI.Models;
using System.Threading.Tasks;
using static BDWebAPI.Services.ProcessorService;

namespace BDWebAPI.Worker
{
    public interface IGeneratorManager
    {
        event GeneratorEventHandler GeneratorEventHandler;

        Task Generate(int batchId, int totalNumberToGenerate);

        Task GetGenerateNumberTask(int batchId);

        void OnNumberGeneration(ProcessorEventArgs generatorEventArgs);
    }
}
