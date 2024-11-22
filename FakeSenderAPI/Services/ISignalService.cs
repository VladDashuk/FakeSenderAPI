namespace FakeSenderAPI.Services
{
    public interface ISignalService
    {
        DataRecord GenerateDataRecord(long timestamp);
    }
}
