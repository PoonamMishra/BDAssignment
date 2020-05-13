using BDWebAPI.Controllers;
using BDWebAPI.Models;
using BDWebAPI.Models.Entities;
using BDWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BDWebAPI.Test
{
    public class BDApiControllerTest : UnitTestData
    {
        protected readonly BDApiController BDApiControllerUnderTest;


        protected readonly Mock<IProcessorService> MockProcessorService;
        protected readonly Mock<ILogger<BDApiController>> MockLoggingService;

        public BDApiControllerTest() : base()
        {
            MockLoggingService = new Mock<ILogger<BDApiController>>();
            MockProcessorService = new Mock<IProcessorService>();
            BDApiControllerUnderTest = new BDApiController(MockLoggingService.Object, MockProcessorService.Object);
        }

        [Fact]
        public async void GetCurrentState_Return_OkResult()
        {
            var result = await BDApiControllerUnderTest.GetCurrentState();
            Assert.IsType<OkObjectResult>(result.Result);


        }

        [Fact]
        public async void GetCurrentState_ReturnResult_Test()
        {

            MockProcessorService.Setup(p => p.GetCurrentState(It.IsAny<int>())).Returns(Task.FromResult(BatchOutput.BatchList));


            ActionResult<BatchOutput> result = await BDApiControllerUnderTest.GetCurrentState(2);

            var viewResult = Assert.IsType<OkObjectResult>(result.Result);
            var batchOutput = Assert.IsType<BatchOutput>(viewResult.Value);

            Assert.False(batchOutput.IsProcessCompleted);
            Assert.NotEmpty(batchOutput.BatchList);
            Assert.Equal(0, batchOutput.CurrentGroupId);

        }

        [Fact]
        public async void GetCurrentState_EmptyResult_Test()
        {

            var emptyList = new List<Batch>();
            MockProcessorService.Setup(p => p.GetCurrentState(It.IsAny<int>())).Returns(Task.FromResult(emptyList.AsEnumerable()));



            ActionResult<BatchOutput> result = await BDApiControllerUnderTest.GetCurrentState(2);

            var viewResult = Assert.IsType<OkObjectResult>(result.Result);
            var batchOutput = Assert.IsType<BatchOutput>(viewResult.Value);

            Assert.False(batchOutput.IsProcessCompleted);
            Assert.True(batchOutput.BatchList.Count() == 0);
            Assert.Equal(0, batchOutput.CurrentGroupId);

        }

        [Fact]
        public async void PerformeCalculation_Input_IsNull_Test()
        {
            var result = await BDApiControllerUnderTest.PerformeCalculation(null);
            Assert.IsType<BadRequestObjectResult>(result);


        }

        [Fact]
        public async void PerformeCalculation_ReturnResult_Test()
        {
            MockProcessorService.Setup(p => p.PerformeCalculation(It.IsAny<BatchInput>())).Returns(Task.FromResult(true));

            var result = await BDApiControllerUnderTest.PerformeCalculation(BatchInput);
            Assert.IsType<OkResult>(result);


        }


    }
}



