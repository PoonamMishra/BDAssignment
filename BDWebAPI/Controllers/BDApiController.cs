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

        
        //[HttpGet("/api/batch/processing")]
        //public async Task<IActionResult> PerformeCalculation1()
        //{
        //    BatchInput input = new BatchInput
        //    {
        //        BatchSize = 5,
        //        ItemsPerBatch = 7
        //    };

        //    await _processorService.PerformeCalculation(input);
        //    return NoContent();
        //}


        [HttpPost("/api/batch/processing")]
        public async Task<IActionResult> PerformeCalculation([FromBody]BatchInput input)
        {
            try
            {
                //BatchInput batchInput = new BatchInput() { BatchCount = 2, ItemsPerBatch = 2 };

                if (input == null)
                {
                    _logger.LogError("input sent from client is null.");
                    return BadRequest("input object is null");
                }

                await _processorService.PerformeCalculation(input);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside PerformeCalculation action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("/api/batch/state/{batchSize:int?}")]
        public async Task<IActionResult> Get([FromQuery]int? batchSize = null)
        {

            var batches = await _processorService.GetCurrentState();

            //bool chkProgress = batches.Count() != 0 && 
            //                          batches.Count() == batchSize &&
            //                          batches.Where(data => data.TotalRemainingItem > 0).FirstOrDefault() != null;

            var response = new
            {
                isProcessCompleted = false,
                groupId = 1,
                batchList = batches,
                total = 100
            };

            return Ok(response);
        }

      
    }
}
