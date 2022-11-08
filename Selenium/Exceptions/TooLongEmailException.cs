namespace Selenium.Exceptions
{
    public class TooLongEmailException : Exception
    {
        public string Email { get; set; }

        public TooLongEmailException(string email)
        {
            Email = email;
        }
    }
}
