using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Selenium.Database;
using Selenium.Exceptions;
using Selenium.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.HostedClasses
{
    public class RunTimeAccountCreator : BackgroundService
    {
        IUsersRepo _usersRepo;

        IAccountGenerationModule accountGenerationModule;

        private float autostop;

        private int betmultiplier;

        private int neededCash;

        public RunTimeAccountCreator(IUsersRepo _usersRepo, IAccountGenerationModule accountGenerationModule, IConfiguration configuration)
        {
            this._usersRepo = _usersRepo;
            this.accountGenerationModule = accountGenerationModule;
            neededCash = configuration.GetValue<int>("NeededCash");
            autostop = configuration.GetValue<float>("DefaultAutoStop");
            betmultiplier = configuration.GetValue<int>("DefaultBetMultiplier");
        }

        public async Task StartAccountCreation()
        {
            while (true)
            {
                try
                {
                    var account = await accountGenerationModule.GenerateAccount(autostop, betmultiplier, neededCash);

                    await _usersRepo.Create(account);

                    Console.WriteLine("Account Created");
                }
                catch (TooLongEmailException)
                {
                    continue;
                }
                catch (RandomApiException)
                {
                    continue;
                }
                catch (APIException e)
                {
                    Console.WriteLine(e.Message);
                }
                //catch (NullReferenceException e)
                //{
                //    Console.WriteLine(e.StackTrace);
                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine(e.StackTrace);
                //}
            }
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await StartAccountCreation();
        }
    }
}
