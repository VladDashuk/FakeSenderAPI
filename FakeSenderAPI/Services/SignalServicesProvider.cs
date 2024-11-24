using System.Collections.Generic;

namespace DataSimulator.Services
{
    public class SignalServicesProvider : ISignalServicesProvider
    {
        private readonly IEnumerable<ISignalService> _signalServices;

        public SignalServicesProvider(IEnumerable<ISignalService> signalServices)
        {
            _signalServices = signalServices;
        }

        public IEnumerable<ISignalService> GetSignalServices()
        {
            return _signalServices;
        }
    }
}
