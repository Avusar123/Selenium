namespace Selenium.Decorators
{
    public class UpdateBalanceAccountLogic : PlaceBetLogicDecorator
    {

        public UpdateBalanceAccountLogic(BetLogic prevlogic = null) : base(prevlogic)
        {
        }

        public async override Task Handler(PlaceBetLogicParams p)
        {
            await base.Handler(p);

            var balance = await p.BetApi.GetBalance(new BalanceRequest() { Token = p.Account.Token });

            p.Account.Balance = balance.Balance;
        }
    }
}
