using Microsoft.Extensions.Hosting;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace FakeSenderAPI.Services
{
    public class DataSimulatorService : IHostedService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<DataSimulatorService> _logger;
        private readonly ISignalService _signalService;
        private Timer? _timer;

        public DataSimulatorService(
            IHttpClientFactory httpClientFactory, ILogger<DataSimulatorService> logger, ISignalService signalService)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _signalService = signalService;
            
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("DataSimulatorService started");
            _timer = new Timer(SendData, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
            return Task.CompletedTask;
        }

        private async void SendData(object? state)
        {
            try
            {
                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                var dataRecord = _signalService.GenerateDataRecord(timestamp);

                var jsonContent = JsonSerializer.Serialize(dataRecord);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var httpClient = _httpClientFactory.CreateClient("MeasurmentClient");
                var response = await httpClient.PostAsync("", content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Data sent succesfully: {jsonContent}");
                }
                else
                {
                    _logger.LogWarning($"Failed to send data: {response.StatusCode}");
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

}

