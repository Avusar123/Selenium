using Microsoft.Extensions.Logging;
using Selenium.Exceptions;

namespace Selenium.Decorators
{
    public class CheckAccountBalanceLogic : PlaceBetLogicDecorator
    {

        int _cashNeeded;

        public CheckAccountBalanceLogic(int CashNeeded, BetLogic prevlogic = null) : base(prevlogic)
        {
            _cashNeeded = CashNeeded;
        }

        public async override Task Handler(PlaceBetLogicParams p)
        {
            await base.Handler(p);

            var account = p.Account;

            var balance = account.Balance;

            p.Logger.LogDebug($"Account {account.Login} has balance {balance}");

            if (balance < 1)
            {
                throw new NotEnoughBalanceException(account);
            }

            if (balance >= _cashNeeded)
            {
                p.Logger.LogInformation($"Account with Login: {account.Login} and Password: {account.Password} has {balance}!");

                throw new NeededCashReachedException(account);
            }

        }
    }
}
