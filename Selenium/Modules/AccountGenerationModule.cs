using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium.DevTools.V104.BackgroundService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Modules
{
    public class AccountGenerationModule : BackgroundService, IAccountGenerationModule
    {
        private ILogger<AccountGenerationModule> logger;

        private IAPI api;

        private async Task<UserData> GenerateAccount()
        {
            var responce = await api.OneClickRegistration(new OneClickRegistrationRequest());

            var userdata = new UserData(responce.Login, responce.Password, responce.Token);

            return userdata;
        }

        private async Task<float> GetBonus(UserData user)
        {
            var responce = await api.StartFreeSpin(new SpinRequest() { Token = user.Token });

            return responce.Bonus;
        }

        public async Task<UserData> GeneratePlayableAccount()
        {
            while (true)
            {
                var user = await GenerateAccount();
                var bonus = await GetBonus(user);
                if (bonus == 1)
                {
                    logger.LogDebug($"New playable account generated with Login: {user.Login} and Password: {user.Password}");
                    return user;
                }
            }
        }

        public async Task<List<UserData>> GeneratePlayableAccount(int count)
        {
            var result = new List<UserData>();

            for (int i = 0; i < count; i++)
            {
                result.Add(await GeneratePlayableAccount());
            }

            return result;
        }

        public async Task<string> GetOneTimeToken()
        {
            var account = await GenerateAccount();
            return account.Token;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }

        public AccountGenerationModule(IAPI api, ILogger<AccountGenerationModule> logger)
        {
            this.logger = logger;
            this.api = api;
        }
    }

    public interface IAccountGenerationModule
    {
        public Task<List<UserData>> GeneratePlayableAccount(int count);

        public Task<UserData> GeneratePlayableAccount();

        public Task<string> GetOneTimeToken();
    }
}
