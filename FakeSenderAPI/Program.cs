using FakeSenderAPI.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<DataSimulatorService>();

builder.Services.AddHttpClient();

var app = builder.Build();

app.Run();
