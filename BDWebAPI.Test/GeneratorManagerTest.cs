using BDWebAPI.ApiContext.Repository;
using BDWebAPI.Models.Entities;
using BDWebAPI.Services;
using BDWebAPI.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BDWebAPI.Test
{
    public class GeneratorManagerTest : UnitTestData
    {
        protected readonly GeneratorManager GeneratorManagerUnderTest;


        protected readonly Mock<IProcessorService> MockProcessorService;
        protected readonly Mock<IBatchRepository> MockBatchRepository;
        protected readonly Mock<ILogger<GeneratorManager>> MockLoggingService;
        protected readonly Mock<BatchContext> MockBatchContext;
        protected readonly Mock<DbSet<Batch>> MockSet;
        private readonly Mock<IGeneratorManager> MockGeneratorManager;

        public GeneratorManagerTest() : base()
        {
            MockLoggingService = new Mock<ILogger<GeneratorManager>>();
            MockGeneratorManager = new Mock<IGeneratorManager>();
            MockProcessorService = new Mock<IProcessorService>();

            MockBatchContext = new Mock<BatchContext>();

            GeneratorManagerUnderTest = new GeneratorManager(MockLoggingService.Object);

            MockSet = new Mock<DbSet<Batch>>();

            var queryable = BatchItems.AsQueryable();

            MockSet.As<IQueryable<Batch>>().Setup(m => m.Provider).Returns(queryable.Provider);
            MockSet.As<IQueryable<Batch>>().Setup(m => m.Expression).Returns(queryable.Expression);
            MockSet.As<IQueryable<Batch>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            MockSet.As<IQueryable<Batch>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator);

        }

        [Fact]
        public void Generate_Return_Result_Test()
        {
            int batchId = 1;
            int totalNumberToGenerate = 2;          

            Task result = GeneratorManagerUnderTest.Generate(batchId, totalNumberToGenerate);
            Assert.IsAssignableFrom<Task>(result);
        }

        //[Fact]
        //public async void GetGenerateNumberTask_ProcessorService_HandlerExecuted()
        //{
        //    // Arrange
        //    MockProcessorService.Setup(x => x.GeneratorCallback(It.IsAny<object>(), 
        //    It.IsAny<ProcessorEventArgs>())).Verifiable();

        //    MockGeneratorManager.Setup(x => x.GetGenerateNumberTask(It.IsAny<int>())).Verifiable();

           

        //    // Act
        //    await GeneratorManagerUnderTest.GetGenerateNumberTask(1);
           
        //    MockGeneratorManager.Raise(m => m.GeneratorEventHandler += null, new ProcessorEventArgs
        //    {
        //        BatchId = 1,
        //        ComputedNumber = 2
        //    });

        //    // Assert
        //    // Assertion to check if the handler does its job.
        //    MockProcessorService.Verify(x => x.GeneratorCallback(It.IsAny<object>(),
        //    It.IsAny<ProcessorEventArgs>()), Times.Once);
            
        //}

        [Fact]
        public void GenerateNumber_Number_RangeCheck_Test()
        {
            int result = GeneratorManagerUnderTest.GenerateNumber();
            Assert.InRange(result, 1, 101);
        }

    }
}

