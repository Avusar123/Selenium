namespace Selenium
{
    public class EmailRegistationResponce
    {
        public EmailRegistrationOriginal Original { get; set; }
    }

    public class EmailRegistrationOriginal
    {
        public string Token { get; set; }
    }

    public class RandomEmailResponce
    {
        public string Email { get; set; }
    }

    public class ChangeNickNameResponce
    {

    }

    public class RandomNameResponce
    {
        public string Name { get; set; }
    }

    public class LoginResponce
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }

    public class SpinResponce
    {
        public float Bonus { get; set; }
    }

    public class CrashHistoryResponce
    {
        public CrashSessionData[] Data { get; set; }
    }

    public class PlaceBetResponce
    {
        public bool IsNextRound { get; set; }

        public string Message { get; set; }
    }

    public class CrashSessionData
    {
        public int Id { get; set; }

        public string State { get; set; }
    }

    public class BalanceResponce
    {
        public float Balance { get; set; }

        public float Bonus { get; set; }
    }
}
