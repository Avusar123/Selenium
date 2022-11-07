using Selenium.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
