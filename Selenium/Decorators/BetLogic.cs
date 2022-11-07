using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Selenium.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Websocket.Client.Logging;

namespace Selenium.Decorators
{
    public abstract class BetLogic
    {
        public abstract Task Handler(PlaceBetLogicParams p);
    }

    public abstract class PlaceBetLogicDecorator : BetLogic
    {
        protected BetLogic prevlogic;

        public PlaceBetLogicDecorator(BetLogic prevlogic)
        {
            this.prevlogic = prevlogic;
        }

        public override async Task Handler(PlaceBetLogicParams p)
        {
            if (prevlogic != null)
            {
                await prevlogic.Handler(p);
            }
        }
    }


    public class PlaceBetLogicParams
    {
        public UserData Account { get; set; }

        public ILogger<BetPlacerModule> Logger { get; set; }

        public IAPI BetApi { get; set; }
    }
}
