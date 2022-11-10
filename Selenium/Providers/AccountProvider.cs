using Microsoft.Extensions.Configuration;
using Selenium.Exceptions;

namespace Selenium.Providers
{
    public class AccountProvider : IAccountGenerationModule
    {
        private IAPI api;

        public async Task<UserData> GenerateAccount(float autostop, int betmultiplier, int neededCash)
        {
            var password = GeneratePassword();

            var email = await api.GetRandomMail(new RandomEmailParseRequest());

            var name = await api.GetRandomName(new RandomNameParseRequest());


            if (email.Email.Length > 30)
            {
                throw new TooLongEmailException(email.Email);
            }

            return await RegisterAccount(email.Email, password, autostop, betmultiplier, neededCash, name.Name);
        }

        private string GeneratePassword()
        {
            return Guid.NewGuid().ToString("N").ToLower().Substring(0, 10);
        }

        private async Task<UserData> RegisterAccount(string email, string password, float autostop, int betmultiplier, int neededCash, string name = null)
        {
            var emailresponce = await api.EmailRegistration(new EmailRegistrationRequest() { Email = email, Password = password });

            if (name != null)
            {
                await api.ChangeNickName(new ChangeNickNameRequest() { Token = emailresponce.Token, Name = name });
            }

            var userdata = new UserData()
            {
                Login = email,
                Password = password,
                Token = emailresponce.Token,
                BetMultiplier = betmultiplier,
                AutoStopRatio = autostop,
                NeededCash = neededCash,
                Balance = 0
            };

            return userdata;
        }

        public async Task<string> GetOneTimeToken()
        {
            var password = GeneratePassword();

            var userdata = await RegisterAccount(password + "@mail.ru", password, 0, 0, 0);

            return userdata.Token;
        }

        public AccountProvider(IAPI api, IConfiguration configuration)
        {
            this.api = api;
        }
    }

    public interface IAccountGenerationModule
    {

        public Task<UserData> GenerateAccount(float autostop, int betmultiplier, int neededCash);

        public Task<string> GetOneTimeToken();
    }
}
