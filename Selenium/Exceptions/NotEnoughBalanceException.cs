namespace Selenium.Exceptions
{
    public class NotEnoughBalanceException : Exception
    {
        public UserData User { get; set; }

        public NotEnoughBalanceException(UserData user)
        {
            User = user;
        }
    }
}
