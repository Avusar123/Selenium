using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Selenium
{
    public class API : IAPI
    {
        private HttpClient client;

        public API()
        {
            client = new HttpClient();
        }

        public async Task<CrashHistoryResponce> GetCrashHistory(CrashHistoryRequest request)
        {
            return await SendRequest<CrashHistoryResponce>(request);
        }

        public async Task<EmailRegistrationOriginal> EmailRegistration(EmailRegistrationRequest request)
        {
            var responce = await SendRequest<EmailRegistationResponce>(request);

            return responce.Original;
        }

        public async Task<SpinResponce> StartFreeSpin(SpinRequest request)
        {
            var auth = new AuthenticationHeaderValue("Bearer", request.Token);

            return await SendRequest<SpinResponce>(request, auth);
        }

        public async Task<BalanceResponce> GetBalance(BalanceRequest request)
        {
            var auth = new AuthenticationHeaderValue("Bearer", request.Token);

            return await SendRequest<BalanceResponce>(request, auth);
        }

        public async Task<PlaceBetResponce> PlaceBet(PlaceBetRequest request)
        {
            var auth = new AuthenticationHeaderValue("Bearer", request.Token);

            return await SendRequest<PlaceBetResponce>(request, auth);
        }

        private async Task<Responce> SendRequest<Responce>(RequestParamsBase parms, AuthenticationHeaderValue auth = null)
            where Responce : class
        {

            var responce = await client.SendAsync(CompileRequestMessage(parms, auth));

            var responcejson = await responce.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Responce>(responcejson);
        }

        private StringContent FormRequestBody(RequestParamsBase parms)
        {
            var content = parms.GetBody();

            return new StringContent(content, Encoding.UTF8, "application/json");
        }

        private HttpRequestMessage CompileRequestMessage(RequestParamsBase parms, AuthenticationHeaderValue auth = null)
        {
            var requestMessage = new HttpRequestMessage(parms.Method, parms.Link);

            if (auth != null)
            {
                requestMessage.Headers.Authorization = auth;
            }

            if (parms.Method == HttpMethod.Post)
            {
                requestMessage.Content = FormRequestBody(parms);
            }

            return requestMessage;
        }

        private async Task<HtmlNodeCollection> ParseHtml(ParseRequest request)
        {
            var responce = await client.GetAsync(request.Link);

            HtmlDocument htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(await responce.Content.ReadAsStringAsync());

            var elementcollection = htmlDoc.DocumentNode.SelectNodes(request.XPathSelector);

            return elementcollection;
        }

        public async Task<LoginResponce> Login(LoginRequest request)
        {
            var auth = new AuthenticationHeaderValue("Bearer", request.Token);

            return await SendRequest<LoginResponce>(request, auth);
        }

        public async Task<RandomEmailResponce> GetRandomMail(RandomEmailParseRequest request)
        {
            var random = new Random();

            var nodecollection = await ParseHtml(request);

            var nickname = nodecollection[0].FirstChild.InnerHtml;

            var email = nickname.ToLower().Replace(" ", "") + random.Next(DateTime.Now.Year - 70, DateTime.Now.Year - 18) + "@gmail.com";

            return new RandomEmailResponce() { Email = email };
        }

        public async Task<RandomNameResponce> GetRandomName(RandomNameParseRequest request)
        {

            var nodecollection = await ParseHtml(request);

            var nickname = nodecollection[0].FirstChild.InnerHtml;

            return new RandomNameResponce() { Name = nickname };
        }

        public async Task<ChangeNickNameResponce> ChangeNickName(ChangeNickNameRequest request)
        {
            var auth = new AuthenticationHeaderValue("Bearer", request.Token);

            return await SendRequest<ChangeNickNameResponce>(request, auth);
        }
    }



    public interface IAPI
    {
        public Task<EmailRegistrationOriginal> EmailRegistration(EmailRegistrationRequest request);

        public Task<RandomEmailResponce> GetRandomMail(RandomEmailParseRequest request);

        public Task<RandomNameResponce> GetRandomName(RandomNameParseRequest request);

        public Task<SpinResponce> StartFreeSpin(SpinRequest request);

        public Task<BalanceResponce> GetBalance(BalanceRequest request);

        public Task<CrashHistoryResponce> GetCrashHistory(CrashHistoryRequest request);

        public Task<PlaceBetResponce> PlaceBet(PlaceBetRequest request);

        public Task<LoginResponce> Login(LoginRequest request);

        public Task<ChangeNickNameResponce> ChangeNickName(ChangeNickNameRequest request);
    }


}
