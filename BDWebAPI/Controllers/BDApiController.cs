using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using BDWebAPI.Models;
using BDWebAPI.Models.Entities;
using BDWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BDWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BDApiController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<BDApiController> _logger;
        private readonly IProcessorService _processorService;

        public BDApiController(ILogger<BDApiController> logger, IProcessorService processorService)
        {
            _logger = logger;
            _processorService = processorService;
        }

        //[HttpGet("/api/batchstate")]
        //public BatchOutput Get1()
        //{

        //    Batch batch1 = new Batch() { BatchId = 1, Total = 5, TotalProcessedItem = 2, TotalRemainingItem = 3 };
        //    Batch batch2 = new Batch() { BatchId = 2, Total = 5, TotalProcessedItem = 2, TotalRemainingItem = 3 };
        //    Batch batch3 = new Batch() { BatchId = 3, Total = 5, TotalProcessedItem = 2, TotalRemainingItem = 3 };
        //    Batch batch4 = new Batch() { BatchId = 4, Total = 5, TotalProcessedItem = 2, TotalRemainingItem = 3 };

        //    List<Batch> batches = new List<Batch>();
        //    batches.Add(batch1);
        //    batches.Add(batch2);
        //    batches.Add(batch3);
        //    batches.Add(batch4);

        //    BatchOutput batchOutput = new BatchOutput()
        //    {
        //        GroupBatchId = 1,
        //        BatchList = batches,
        //        Total = 100
        //    };

        //    return batchOutput;



        //}

        
        [HttpGet("/api/batchstate")]
        public IEnumerable<Batch> Get()
        {

            IEnumerable<Batch> batches= _processorService.GetCurrentState();
            return batches;
        }

        [HttpPost("/api/startprocessing")]
        public string PerformeCalculation(BatchInput input)
        {
            return _processorService.PerformeCalculation(input);


        }

        [HttpGet("/api/processing")]
        public string PerformeCalculation1()
        {
            BatchInput batchInput = new BatchInput() { BatchCount = 2, ItemPerBatch = 2 };

            return _processorService.PerformeCalculation(batchInput);


        }
    }
}
