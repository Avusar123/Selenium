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
using Selenium.Decorators;
using Selenium.Exceptions;
using Selenium.Database;

namespace Selenium.Modules
{
    public class BetPlacerModule : IBetPlacerModule
    {

        private IAPI api;

        private ILogger<BetPlacerModule> logger;

        public BetPlacerModule(IAPI api, ILogger<BetPlacerModule> logger)
        {
            this.api = api;
            this.logger = logger;
        }

        public async Task PlaceBetForAllAccounts(int roundId,params UserData[] accounts)
        {
            foreach (var account in accounts)
            {
                try
                {
                    await new PlaceGeneratedAccountBetLogic(account.BetMultiplier, roundId,
                    new CheckAccountBalanceLogic(account.NeededCash,
                        new UpdateBalanceAccountLogic())).Handler(new PlaceBetLogicParams() { BetApi = api, Account = account, Logger = logger });
                } catch (NotEnoughBalanceException)
                {
                    continue;
                } catch (NeededCashReachedException)
                {
                    continue;
                }
            }
        }
    }

    public interface IBetPlacerModule
    {
        public Task PlaceBetForAllAccounts(int roundId, params UserData[] accounts);
    }
}
