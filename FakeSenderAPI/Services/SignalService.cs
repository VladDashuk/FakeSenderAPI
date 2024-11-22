namespace FakeSenderAPI.Services
{
    public class SignalService : ISignalService
    {
        public DataRecord GenerateDataRecord(long timestamp)
        {
            return new DataRecord
            {
                id = new Random().Next(0,10),
                wattage = new Random().Next(100, 500),
                timestamp = DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime
            };
        }
    }
}
