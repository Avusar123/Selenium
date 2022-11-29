using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Selenium.Database;
using Selenium.Exceptions;
using Selenium.Modules;
using Selenium.Providers;

namespace Selenium.HostedClasses
{
    public class BetBot : BackgroundService
    {
        IAccountGenerationModule accountGenerationModule;

        ISocketModule socketModule;

        IBetPlacerModule betPlacerModule;

        IUsersRepo usersRepo;

        ILogger<BetBot> logger;

        private CrashGame crashGame;

        private CancellationTokenSource BetTimeExpiredCancellation = new CancellationTokenSource();

        public Task OnGameStarted(object? sender, CrashGame e)
        {
            crashGame = e;
            return Task.CompletedTask;
        }

        private async Task PlaceBetsAsync()
        {
            var accounts = await usersRepo.GetAll(user => user.Balance >= 1 && user.Balance < user.NeededCash);
            logger.LogDebug($"Available accounts: {accounts.Count}");
            await betPlacerModule.PlaceBetForAllAccounts(crashGame.gameId, BetTimeExpiredCancellation.Token, accounts.ToArray());
            await usersRepo.Save();
        }

        public async Task OnGameStateChanged(object? sender, string e)
        {
            if (e == "timer")
            {
                logger.LogDebug("Timer Started");
                await Task.Run(PlaceBetsAsync);
            }

            if (e == "game")
            {
                logger.LogDebug("Game Started");
                if (BetTimeExpiredCancellation != null && !BetTimeExpiredCancellation.IsCancellationRequested)
                {
                    BetTimeExpiredCancellation.Cancel();
                    BetTimeExpiredCancellation = new CancellationTokenSource();
                }
            }
        }

        //public async Task UpdateAccountsBalance(params UserData[] accounts)
        //{
        //    foreach (var account in accounts)
        //    {
        //        account.Balance = (await api.GetBalance(new BalanceRequest() { Token = account.Token })).Balance;
        //    }

        //    await usersRepo.Save();
        //}

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var accounts = await usersRepo.GetAll();
            socketModule.CrashCreated += OnGameStarted;
            socketModule.GameStateChaged += OnGameStateChanged;
            await socketModule.Connect(await accountGenerationModule.GetOneTimeToken());
        }


        public BetBot(IAccountGenerationModule accountGenerationModule, ISocketModule socketModule,
            IUsersRepo usersRepo, IBetPlacerModule betPlacerModule, ILogger<BetBot> logger)
        {
            this.accountGenerationModule = accountGenerationModule;
            this.socketModule = socketModule;
            this.usersRepo = usersRepo;
            this.betPlacerModule = betPlacerModule;
            this.logger = logger;
        }
    }
}
