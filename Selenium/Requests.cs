using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Selenium
{
    public class OneClickRegistrationRequest : RequestParamsBase
    {
        public override string Link => "https://api.getx.pro/auth/register/one_click";

        public override HttpMethod Method => HttpMethod.Post;
    }

    public class BalanceRequest : RequestParamsBase
    {
        public override string Link => "https://api.getx.bingo/profile/balance";

        public override HttpMethod Method => HttpMethod.Get;

        public string Token { get; set; }
    }

    public class SpinRequest : RequestParamsBase
    {
        public override string Link => "https://api.getx.bingo/bonuses/roulette/spin";

        public override HttpMethod Method => HttpMethod.Post;

        public string Token { get; set; }

        public override string GetBody()
        {
            var serializerSettings = new JsonSerializerSettings();

            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            var obj = new { token = Token };

            return JsonConvert.SerializeObject(obj, serializerSettings);
        }
    }

    public class CrashHistoryRequest : RequestParamsBase
    {
        public override string Link => "https://api.getx.bingo/games/crash/history";

        public override HttpMethod Method => HttpMethod.Get;
    }

    public class PlaceBetRequest : RequestParamsBase
    {
        public override string Link => "https://api.getx.bingo/crash/placeBet";

        public override HttpMethod Method => HttpMethod.Post;

        public string Token { get; set; }

        public int RoundId { get; set; }

        public float BetAmount { get; set; }

        public float AutoStopRatio { get; set; }

        public override string GetBody()
        {
            var serializerSettings = new JsonSerializerSettings();

            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            var obj = new { betAmount = BetAmount, autoStopRatio = AutoStopRatio, roundId = RoundId };

            return JsonConvert.SerializeObject(obj, serializerSettings);
        }
    }

    public abstract class RequestParamsBase
    {
        public abstract string Link { get; }

        public abstract HttpMethod Method { get; }

        public virtual string GetBody()
        {
            return String.Empty;
        }
    }
}
