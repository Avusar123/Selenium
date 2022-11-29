using Microsoft.Extensions.Logging;
using Selenium.Decorators;
using Selenium.Exceptions;

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

        public async Task PlaceBetForAllAccounts(int roundId, CancellationToken token ,params UserData[] accounts)
        {
            foreach (var account in accounts)
            {
                try
                {
                    await new PlaceGeneratedAccountBetLogic(account.BetMultiplier, roundId,
                    new CheckAccountBalanceLogic(account.NeededCash,
                        new UpdateBalanceAccountLogic())).Handler(new PlaceBetLogicParams() { BetApi = api, Account = account, Logger = logger });

                    if (token.IsCancellationRequested)
                    {
                        return;
                    }
                }
                catch (NotEnoughBalanceException)
                {
                    continue;
                }
                catch (NeededCashReachedException)
                {
                    continue;
                }
            }
        }
    }

    public interface IBetPlacerModule
    {
        public Task PlaceBetForAllAccounts(int roundId, CancellationToken token, params UserData[] accounts);
    }
}
