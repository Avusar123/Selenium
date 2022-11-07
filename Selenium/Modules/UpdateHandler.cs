using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Selenium.Database;
using Selenium.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Modules
{
    public class UpdateHandler : BackgroundService, IUpdateHandler
    {
        IAccountGenerationModule accountGenerationModule;

        ISocketModule socketModule;

        IBetPlacerModule betPlacerModule;

        IUsersRepo usersRepo;

        ILogger<UpdateHandler> logger;

        IAPI api;

        public async Task OnGameStarted(object? sender, CrashGame e)
        {
            
            await betPlacerModule.PlaceBetForAllAccounts(e.gameId, (await usersRepo.GetAll(user => user.Balance >= 1 && user.Balance < user.NeededCash)).ToArray());
            await usersRepo.Save();
        }

        public async Task OnSpin(params UserData[] accounts)
        {
            foreach (var account in accounts)
            {
                var spinresponce = await api.StartFreeSpin(new SpinRequest() { Token = account.Token });

                account.Balance += spinresponce.Bonus;

                logger.LogDebug($"Account has spinned and now has balance {account.Balance}");

            }
        }

        public async Task StartAccountCreation()
        {
            while (true)
            {
                try
                {
                    var account = await accountGenerationModule.GenerateAccount(3, 2);
                    await OnSpin(account);

                    await usersRepo.Create(account);
                }
                catch (TooLongEmailException)
                {
                    continue;
                }
                catch (NullReferenceException)
                {
                    continue;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                
            }
        }

        public async Task UpdateAccountsBalance(params UserData[] accounts)
        {
            foreach (var account in accounts)
            {
                account.Balance = (await api.GetBalance(new BalanceRequest() { Token = account.Token })).Balance;
            }

            await usersRepo.Save();
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var accounts = await usersRepo.GetAll();
            //await UpdateAccountsBalance(accounts.ToArray());
            socketModule.CrashCreated += OnGameStarted;
            await socketModule.Connect(accounts[0].Token);
            Task.Run(StartAccountCreation, stoppingToken);
        }

        public UpdateHandler(IAccountGenerationModule accountGenerationModule, ISocketModule socketModule, IAPI api, IUsersRepo usersRepo, IBetPlacerModule betPlacerModule, ILogger<UpdateHandler> logger)
        {
            this.accountGenerationModule = accountGenerationModule;
            this.socketModule = socketModule;
            this.api = api;
            this.usersRepo = usersRepo;
            this.betPlacerModule = betPlacerModule;
            this.logger = logger;
        }
    }

    public interface IUpdateHandler
    {
        public Task OnGameStarted(object? sender, CrashGame e);

        public Task StartAccountCreation();

        public Task OnSpin(params UserData[] accounts);

        public Task UpdateAccountsBalance(params UserData[] accounts);
    }
}
