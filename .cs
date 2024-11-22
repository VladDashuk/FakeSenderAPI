using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Text;
using System.Text.Json;

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
            var filePath = "data.json";
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"File not found: {filePath}");
                return;
            }

            var json = await File.ReadAllTextAsync(filePath);
            var dataRecords = JsonSerializer.Deserialize<List<DataRecord>>(json);

            if (dataRecords == null || dataRecords.Count == 0)
            {
                _logger.LogWarning("No data found in JSON.");
                return;
            }

            foreach (var record in dataRecords)
            {
                DateTime fullDateTime = DateTime.Parse(record.Timestamp);
                TimeSpan timeOnly = fullDateTime.TimeOfDay;

                var payload = new
                {
                    wattage = record.Wattage,
                    time = timeOnly.ToString()
                };

                var httpClient = _httpClientFactory.CreateClient();
                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("http://measurement-service/api/measurements", content);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Data sent successfully: {payload}");
                }
                else
                {
                    _logger.LogWarning($"Failed to send data: {response.StatusCode}");
                }
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
    public float Wattage { get; set; }
    public string Timestamp { get; set; }
}