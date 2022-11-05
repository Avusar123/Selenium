using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Principal;

namespace Selenium.Modules
{
    public class BetBot : BackgroundService
    {
        private ISocketModule apiSocket;

        private IAPI api;

        private IAccountGenerationModule accountGenerationModule;

        private List<UserData> accountList;

        //private const int amountOfAccounts = 1;

        private const int betAmountMultiplier = 2;

        private const int CashNeeded = 10;

        private ILogger<BetBot> logger;

        public BetBot(ISocketModule apiSocket, IAccountGenerationModule accountGenerationModule, IAPI api, ILogger<BetBot> logger)
        {
            this.apiSocket = apiSocket;
            this.api = api;
            this.accountGenerationModule = accountGenerationModule;
            this.logger = logger;
            accountList = new();
        }

        private async Task InitSocket()
        {
            apiSocket.CrashCreated += UpdateGame;
            apiSocket.GameStateChaged += GameStateChaged;
            await apiSocket.Connect();
        }

        private async Task GameStateChaged(object? sender, string e)
        {
            if (e == "complete")
            {
                accountList.RemoveAll((userData) => userData.Balance < 1);

                logger.LogDebug($"Accounts were deleted");
            }
        }

        private async Task PlaceBetForAllAccounts(int roundId)
        {
            Console.WriteLine(accountList.Count);

            foreach (var account in accountList)
            {
                var balance = await api.GetBalance(new BalanceRequest() { Token = account.Token });

                account.Balance = balance.Balance;

                logger.LogDebug($"Account {account.Login} has balance {balance.Balance}");

                if (balance.Balance < 1)
                {   
                    continue;
                }

                if (balance.Balance >= CashNeeded)
                {
                    logger.LogInformation($"Account with Login: {account.Login} and Password: {account.Password} has {balance.Balance}!");
                    accountList.Remove(account);
                    continue;
                }

                float betamount;

                if (balance.Balance == 1)
                {
                    betamount = balance.Balance;
                } else
                {
                    betamount = (float)Math.Round((double)(balance.Balance / betAmountMultiplier), 0, MidpointRounding.ToPositiveInfinity);
                }

                await api.PlaceBet(new() { AutoStopRatio = 2, BetAmount = betamount, RoundId = roundId, Token = account.Token });

                logger.LogDebug($"Account {account.Login} placed bet for {betamount}");
            }
        }

        private async Task StartUpdatingAccoutList()
        {
            while (true)
            {
                var account = await accountGenerationModule.GeneratePlayableAccount();
                account.Balance = 1;
                accountList.Add(account);
            }

            logger.LogDebug("Updated List Of Accounts");
        }

        private async Task UpdateGame(object? sender, CrashGame e)
        {
            var crashgameid = e.gameId;
            if (accountList.Count > 0)
            {
                await PlaceBetForAllAccounts(crashgameid);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            StartUpdatingAccoutList();
            await InitSocket();
        }
    }
}
