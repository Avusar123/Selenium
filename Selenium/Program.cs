using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Selenium;
using Selenium.Database;
using Selenium.Modules;

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