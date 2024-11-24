namespace DataSimulator.Services
{
    public class RandomSignalService : ISignalService
    {
        private string _userId;

        public RandomSignalService(string userId)
        {
            _userId = userId;
        }

        public DataRecord GenerateDataRecord(long timestamp)
        {
            return new DataRecord
            {
                UserId = _userId,
                Wattage = new Random().Next(100, 500),
                Timestamp = timestamp
            };
        }
    }
}
