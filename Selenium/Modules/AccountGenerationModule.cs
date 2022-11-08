using Selenium.Exceptions;

namespace Selenium.Modules
{
    public class AccountGenerationModule : IAccountGenerationModule
    {
        private IAPI api;

        const int NeededCash = 15;

        public async Task<UserData> GenerateAccount(float autostop, int betmultiplier)
        {
            var password = GeneratePassword();

            var email = await api.GetRandomMail(new RandomEmailRequest());

            if (email.Email.Length > 30)
            {
                throw new TooLongEmailException(email.Email);
            }

            return await RegisterAccount(email.Email, password, autostop, betmultiplier);
        }

        private string GeneratePassword()
        {
            return Guid.NewGuid().ToString("N").ToLower().Substring(0, 10);
        }

        private async Task<UserData> RegisterAccount(string email, string password, float autostop, int betmultiplier)
        {
            var emailresponce = await api.EmailRegistration(new EmailRegistrationRequest() { Email = email, Password = password });

            var userdata = new UserData()
            {
                Login = email,
                Password = password,
                Token = emailresponce.Token,
                BetMultiplier = betmultiplier,
                NeededCash = NeededCash,
                AutoStopRatio = autostop,
                Balance = 0
            };

            return userdata;
        }

        public async Task<string> GetOneTimeToken()
        {
            var password = GeneratePassword();

            var userdata = await RegisterAccount(password + "@mail.ru", password, 0, 0);

            return userdata.Token;
        }

        public AccountGenerationModule(IAPI api)
        {
            this.api = api;
        }
    }

    public interface IAccountGenerationModule
    {

        public Task<UserData> GenerateAccount(float autostop, int betmultiplier);

        public Task<string> GetOneTimeToken();
    }
}
