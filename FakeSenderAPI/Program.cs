using DataSimulator.Services;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<ISignalService>(provider => new RandomSignalService("RandomUser"));
        services.AddSingleton<ISignalService>(provider => new StaticSignalService("7777", 666));

        services.AddSingleton<ISignalServicesProvider, SignalServicesProvider>();

        services.AddHttpClient("MeasurementClient", client =>
        {
            var baseUrl = context.Configuration["MeasurementService:BaseUrl"];
            client.BaseAddress = new Uri(baseUrl);
        });

        services.AddHostedService<DataSimulatorService>();
    });

await builder.RunConsoleAsync();
