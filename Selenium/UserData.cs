namespace Selenium
{
    public class UserData
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }

        public float Balance { get; set; }

        public UserData(string login, string password, string token)
        {
            Login = login;
            Password = password;
            Token = token;
            Balance = 0;
        }
    }
}