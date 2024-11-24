
namespace DataSimulator.Services
{
    public interface ISignalServicesProvider
    {
        IEnumerable<ISignalService> GetSignalServices();
    }
}