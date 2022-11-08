namespace Selenium.Exceptions
{
    public class NeededCashReachedException : Exception
    {
        public UserData User { get; set; }

        public NeededCashReachedException(UserData user)
        {
            User = user;
        }
    }
}
