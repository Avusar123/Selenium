using Selenium;
using Selenium.Modules;
using SocketIOClient;
using System.Net.Http.Headers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Selenium.Database;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices(
    services =>
        services
            .AddSingleton<IBetPlacerModule, BetPlacerModule>()
            .AddScoped<IAPI, API>()
            .AddSingleton<ISocketModule, SocketModule>()
            .AddSingleton<IAccountGenerationModule, AccountGenerationModule>()
            .AddScoped<IUsersRepo, UsersRepo>()
            .AddDbContext<AccountContext>(contextLifetime: ServiceLifetime.Scoped, optionsLifetime: ServiceLifetime.Scoped)
            .AddHostedService<UpdateHandler>()
);

builder.ConfigureLogging(loggerbuilder =>
{
    loggerbuilder.SetMinimumLevel(LogLevel.Debug);
});


builder.Build().Run();