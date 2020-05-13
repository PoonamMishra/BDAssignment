using BDWebAPI.ApiContext.Repository;
using BDWebAPI.Models;
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
    public class ProcessorServiceTest : UnitTestData
    {
        protected readonly ProcessorService ProcessorServiceUnderTest;


        protected readonly Mock<IProcessorService> MockProcessorService;
        protected readonly Mock<IGeneratorManager> MockGeneratorManager;
        protected readonly Mock<IMultiplierManager> MockMultiplierManager;
        protected readonly Mock<IBatchRepository> MockBatchRepository;
        protected readonly Mock<ILogger<ProcessorService>> MockLoggingService;
        protected readonly Mock<BatchContext> MockBatchContext;
        protected readonly Mock<DbSet<Batch>> MockSet;

        public ProcessorServiceTest() : base()
        {
            MockLoggingService = new Mock<ILogger<ProcessorService>>();
            MockGeneratorManager = new Mock<IGeneratorManager>();
            MockMultiplierManager = new Mock<IMultiplierManager>();
            MockBatchRepository = new Mock<IBatchRepository>();
            MockProcessorService = new Mock<IProcessorService>();


            MockBatchContext = new Mock<BatchContext>();

            ProcessorServiceUnderTest = new ProcessorService(MockGeneratorManager.Object, MockMultiplierManager.Object,
                 MockBatchRepository.Object, MockLoggingService.Object);

            MockSet = new Mock<DbSet<Batch>>();

            var queryable = BatchItems.AsQueryable();

            MockSet.As<IQueryable<Batch>>().Setup(m => m.Provider).Returns(queryable.Provider);
            MockSet.As<IQueryable<Batch>>().Setup(m => m.Expression).Returns(queryable.Expression);
            MockSet.As<IQueryable<Batch>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            MockSet.As<IQueryable<Batch>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator);

        }

        [Fact]
        public void PerformeCalculation_Return_Result()
        {

            MockGeneratorManager.Setup(c => c.Generate(
            It.IsAny<int>(),
            It.IsAny<int>()
            )).Returns(() => Task.FromResult(System.Net.HttpStatusCode.OK));


            Task result = ProcessorServiceUnderTest.PerformeCalculation(BatchInput);
            Assert.IsAssignableFrom<Task>(result);
        }

        [Fact]
        public async void Start_SomthingHappenedInvoked_HandlerExecuted()
        {
            // Arrange
            MockProcessorService.Setup(x => x.PerformeCalculation(BatchInput)).Verifiable();
            MockGeneratorManager.Setup(x => x.GetGenerateNumberTask(It.IsAny<int>())).Verifiable();

            // Act
            await ProcessorServiceUnderTest.PerformeCalculation(BatchInput);
            // This will fire the event SomethingHappened from m_dependencyMock.
            MockGeneratorManager.Raise(m => m.GeneratorEventHandler += null, new ProcessorEventArgs
            {
                BatchId = 1,
                ComputedNumber = 2
            });

            // Assert
            // Assertion to check if the handler does its job.
            MockGeneratorManager.Verify(x => x.GetGenerateNumberTask(It.IsAny<int>()), Times.Once);
        }

    }
}

