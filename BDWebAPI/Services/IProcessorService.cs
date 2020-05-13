﻿using BDWebAPI.Models;
using BDWebAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDWebAPI.Services
{
    public interface IProcessorService
    {
        Task<IEnumerable<Batch>> GetCurrentState(int? groupId = null);

        Task<IEnumerable<Batch>> GetPreviousBatch();

        Task PerformeCalculation(BatchInput input);
        Task<IEnumerable<Batch>> GetAllBatches();

        void GeneratorCallback(object sender, ProcessorEventArgs args);
    }
}
