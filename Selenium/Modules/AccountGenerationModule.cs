using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Selenium.Database;
using Selenium.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Modules
{
    public class AccountGenerationModule : IAccountGenerationModule
    {
        private IAPI api;

        const int NeededCash = 15;

        public async Task<UserData> GenerateAccount(float autostop, int betmultiplier)
        {
            var password = Guid.NewGuid().ToString("N").ToLower().Substring(0, 10);

            var email = await api.GetRandomMail(new RandomEmailRequest());

            if (email.Email.Length > 30)
            {
                throw new TooLongEmailException(email.Email);
            }
            
            var emailresponce = await api.EmailRegistration(new EmailRegistrationRequest() { Email = email.Email, Password = password });

            var userdata = new UserData() { Login = email.Email, Password = password, 
                Token = emailresponce.Token, BetMultiplier = betmultiplier, NeededCash = NeededCash, AutoStopRatio = autostop, Balance = 0 };

            return userdata;
        }

        public AccountGenerationModule(IAPI api)
        {
            this.api = api;
        }
    }

    public interface IAccountGenerationModule
    {

        public Task<UserData> GenerateAccount(float autostop, int betmultiplier);
    }
}
