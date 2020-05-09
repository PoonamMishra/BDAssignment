using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
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

        [HttpGet("/api/batchstate/{groupId:int?}")]
        public async Task<IActionResult> Get(int? groupId = null)
        {
            try
            {

                Batch batch1 = new Batch() { BatchId = 1, Total = 5, TotalProcessedItem = 2, TotalRemainingItem = 3 };
                Batch batch2 = new Batch() { BatchId = 2, Total = 5, TotalProcessedItem = 2, TotalRemainingItem = 3 };
                Batch batch3 = new Batch() { BatchId = 3, Total = 5, TotalProcessedItem = 2, TotalRemainingItem = 3 };
                Batch batch4 = new Batch() { BatchId = 4, Total = 5, TotalProcessedItem = 2, TotalRemainingItem = 3 };

                List<Batch> batches = new List<Batch>();
                batches.Add(batch1);
                batches.Add(batch2);
                batches.Add(batch3);
                batches.Add(batch4);

                var response = new
                {
                    isProcessCompleted = false,
                    groupBatchId = 1,
                    batchList = batches,
                    total = 100
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Get action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }




    




    [HttpPost("/api/startprocessing")]
    public async Task PerformeCalculation(BatchInput input)
    {
         await _processorService.PerformeCalculation(input);


    }

    //[HttpGet("/api/batchstate")]
    //public IEnumerable<Batch> Get()
    //{

    //    IEnumerable<Batch> batches= _processorService.GetCurrentState();
    //    return batches;
    //}

    [HttpGet("/api/processing")]
    public async Task PerformeCalculation1()
    {
        BatchInput batchInput = new BatchInput() { BatchCount = 2, ItemPerBatch = 2 };

        await _processorService.PerformeCalculation(batchInput);


    }
}
}
