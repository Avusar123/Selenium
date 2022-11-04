using Selenium;
using Selenium.Modules;
using SocketIOClient;
using System.Net.Http.Headers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices(
    services =>
        services
            .AddHostedService<BetBot>()
            .AddScoped<IAPI, API>()
            .AddSingleton<ISocketModule, SocketModule>()
            .AddSingleton<IAccountGenerationModule, AccountGenerationModule>()
);

builder.ConfigureLogging(loggerbuilder =>
{
    loggerbuilder.SetMinimumLevel(LogLevel.Debug);
});


builder.Build().Run();