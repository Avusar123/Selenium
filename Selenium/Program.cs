using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Selenium;
using Selenium.Database;
using System.Configuration;
using Selenium.Modules;
using Microsoft.Extensions.Configuration;
using Selenium.Providers;
using Selenium.HostedClasses;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices(
    services =>
        services
            .AddHostedService<BetBot>()
            .AddHostedService<AccountSpinner>()
            .AddHostedService<RunTimeAccountCreator>()
            .AddSingleton<IBetPlacerModule, BetPlacerModule>()
            .AddScoped<IAPI, API>()
            .AddSingleton<ISocketModule, SocketModule>()
            .AddSingleton<IAccountGenerationModule, AccountProvider>()
            .AddTransient<IUsersRepo, UsersRepo>()
            .AddDbContext<AccountContext>(contextLifetime: ServiceLifetime.Transient, optionsLifetime: ServiceLifetime.Transient)
            .AddSingleton<IConfiguration>(new ConfigurationManager().AddJsonFile("appsettings.json").Build())
);

builder.ConfigureLogging(loggerbuilder =>
{
    loggerbuilder.SetMinimumLevel(LogLevel.Debug);
});

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Build().Run();