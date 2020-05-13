using BDWebAPI.Models;
using BDWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BDWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BDApiController : ControllerBase
    {

        private readonly ILogger<BDApiController> _logger;
        private readonly IProcessorService _processorService;



        public BDApiController(ILogger<BDApiController> logger, IProcessorService processorService)
        {
            _logger = logger;
            _processorService = processorService;
        }


        [HttpGet("/api/batch/processing")]
        public async Task<IActionResult> PerformeCalculation1()
        {
            BatchInput input = new BatchInput
            {
                BatchSize = 5,
                ItemsPerBatch = 7
            };

            await _processorService.PerformeCalculation(input);
            return NoContent();
        }


        [HttpPost("/api/batch/processing")]
        public async Task<IActionResult> PerformeCalculation([FromBody]BatchInput input)
        {
            try
            {


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

        [HttpGet("/api/batch/state/{groupId?}")]
        public async Task<ActionResult<BatchOutput>> GetCurrentState(int? groupId = null)
        {


            var batches = await _processorService.GetCurrentState(groupId);



            var response = new BatchOutput()
            {
                IsProcessCompleted = ProcessorService.IsProcessCompleted,
                CurrentGroupId = ProcessorService.GroupId,
                BatchList = batches
            };

            return Ok(response);
        }


        [HttpGet("/api/batch/currentgroupid")]
        public int GetCurrentGroupId()
        {
            return ProcessorService.GroupId;
        }

        [HttpGet("/api/batch/batches")]
        public async Task<IActionResult> GetAllBatches()
        {

            var batches = await _processorService.GetAllBatches();

            return Ok(batches);
        }


        [HttpGet("/api/batch/previous")]
        public async Task<IActionResult> GetPreviousBatch()
        {

            var batches = await _processorService.GetPreviousBatch();

            var response = new
            {
                batchList = batches
            };

            return Ok(response);
        }


    }
}
