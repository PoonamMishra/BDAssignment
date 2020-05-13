using static BDWebAPI.Services.ProcessorService;

namespace BDWebAPI.Worker
{
    public interface IMultiplierManager
    {
        event MultiplierEventHandler MultiplierEventHandler;

        void Multiplier(int batchId, int number);

    }
}
