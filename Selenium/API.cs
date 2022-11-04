using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json.Serialization;
using OpenQA.Selenium.DevTools.V104.Page;

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

        public async Task<OneClickRegistrationResponce> OneClickRegistration(OneClickRegistrationRequest request)
        {
            return await SendRequest<OneClickRegistrationResponce>(request);
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
    }



    public interface IAPI
    {
        public Task<OneClickRegistrationResponce> OneClickRegistration(OneClickRegistrationRequest request);

        public Task<SpinResponce> StartFreeSpin(SpinRequest request);

        public Task<BalanceResponce> GetBalance(BalanceRequest request);

        public Task<CrashHistoryResponce> GetCrashHistory(CrashHistoryRequest request);

        public Task<PlaceBetResponce> PlaceBet(PlaceBetRequest request);
    }

    
}
