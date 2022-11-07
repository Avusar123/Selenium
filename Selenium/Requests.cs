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
    public class EmailRegistrationRequest : RequestParamsBase
    {
        public override string Link => "https://api.getx.pro/auth/register/email";

        public override HttpMethod Method => HttpMethod.Post;

        public string Email { get; set; }

        public string Password { get; set; }

        public override string GetBody()
        {
            var serializerSettings = new JsonSerializerSettings();

            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            var obj = new { email = Email, password = Password};

            return JsonConvert.SerializeObject(obj, serializerSettings);
        }
    }

    public class RandomEmailRequest : RequestParamsBase
    {
        public override string Link => "https://generatefakename.com/ru/email";

        public override HttpMethod Method => HttpMethod.Get;
    }


    public class LoginRequest : RequestParamsBase
    {
        public override string Link => "https://api.getx.pro/profile/user";

        public override HttpMethod Method => HttpMethod.Get;

        public string Token { get; set; }
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

    public abstract class ParseRequest : RequestParamsBase
    {
        public abstract string XPathSelector { get; }
    }

    public class RandomEmailParseRequest : ParseRequest
    {
        public override string Link => "https://randomus.ru/name?type=101&sex=10&count=1";

        public override HttpMethod Method => HttpMethod.Get;

        public override string XPathSelector => "//*[@id=\"result_tiles\"]/div/div/div/span";
    }
}
