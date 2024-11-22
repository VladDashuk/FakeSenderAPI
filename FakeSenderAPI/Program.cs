using FakeSenderAPI.Services;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<ISignalService, SignalService>();

        services.AddHttpClient("MeasurementClient", client =>
        {
            var configuration = context.Configuration.GetSection("MeasurementService");
            client.BaseAddress = new Uri(configuration["BaseUrl"]);
        });

        services.AddHostedService<DataSimulatorService>();
    });

await builder.RunConsoleAsync();
