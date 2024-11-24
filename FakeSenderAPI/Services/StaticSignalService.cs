using DataSimulator.Services;

public class StaticSignalService : ISignalService
{
    private readonly string _userId;       
    private readonly float _consumption;  

    public StaticSignalService(string userId, float consumption)
    {
        _userId = userId;
        _consumption = consumption;
    }

    public DataRecord GenerateDataRecord(long timestamp)
    {
        return new DataRecord
        {
            UserId = _userId, 
            Wattage = _consumption,
            Timestamp = timestamp
        };
    }
}
