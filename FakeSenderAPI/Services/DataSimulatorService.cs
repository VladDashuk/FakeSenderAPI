using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Text;
using System.Text.Json;


namespace FakeSenderAPI.Services {

public class DataSimulatorService : IHostedService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<DataSimulatorService> _logger;
    private Timer? _timer;

    public DataSimulatorService(IHttpClientFactory httpClientFactory, ILogger<DataSimulatorService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("DataSimulatorService started.");
        
        _timer = new Timer(SendData, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));

        return Task.CompletedTask;
    }

        private async void SendData(object? state)
        {
            try
            {
                int userId = 1; 

                while (true) 
                {
                  
                    var payload = new
                    {
                        id = Guid.NewGuid().ToString(),        
                        userId = $"User_{userId}",            
                        timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(), 
                        wattage = new Random().Next(100, 500)  
                    };

                    var jsonContent = JsonSerializer.Serialize(payload);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var httpClient = _httpClientFactory.CreateClient();
                    var response = await httpClient.PostAsync("http://localhost:8765/api/Measurement", content);

                    if (response.IsSuccessStatusCode)
                    {
                        _logger.LogInformation($"Data sent successfully: {jsonContent}");
                    }
                    else
                    {
                        _logger.LogWarning($"Failed to send data: {response.StatusCode}");
                    }

                    userId++;
                    if (userId > 10) userId = 1;

                    await Task.Delay(5000); 
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while sending data: {ex.Message}");
            }
        }


        public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("DataSimulatorService stopped.");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }
}


public class DataRecord
{
    public float wattage { get; set; }
    public int id { get; set; } 
    public DateTime timestamp { get; set; }
}

}