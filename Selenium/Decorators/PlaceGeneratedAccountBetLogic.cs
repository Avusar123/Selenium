using Microsoft.Extensions.Logging;

namespace Selenium.Decorators
{
    public class PlaceGeneratedAccountBetLogic : PlaceBetLogicDecorator
    {
        private int BetAmountMultiplier;

        private int RoundId;

        public PlaceGeneratedAccountBetLogic(int BetAmountMultiplier, int RoundId, BetLogic prevlogic = null) : base(prevlogic)
        {
            this.BetAmountMultiplier = BetAmountMultiplier;

            this.RoundId = RoundId;
        }

        public async override Task Handler(PlaceBetLogicParams p)
        {
            await base.Handler(p);

            var betamount = (float)Math.Round((double)(p.Account.Balance / BetAmountMultiplier), 0, MidpointRounding.ToPositiveInfinity);

            var responce = await p.BetApi.PlaceBet(new() { AutoStopRatio = p.Account.AutoStopRatio, BetAmount = betamount, RoundId = RoundId, Token = p.Account.Token });

            p.Logger.LogDebug($"Account {p.Account.Login} placed bet for {betamount}");
        }
    }
}
