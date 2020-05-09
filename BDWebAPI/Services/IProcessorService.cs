using BDWebAPI.Models;
using BDWebAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDWebAPI.Services
{
    public interface IProcessorService
    {
        BatchOutput GetCurrentState1();

         Task<IEnumerable<Batch>> GetCurrentState();

        Task PerformeCalculation(BatchInput input);
    }
}
