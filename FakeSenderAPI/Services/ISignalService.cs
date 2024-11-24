namespace DataSimulator.Services
{
    public interface ISignalService
    {
        DataRecord GenerateDataRecord(long timestamp);
    }
}
