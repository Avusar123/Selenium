namespace Selenium
{
    public class UserData
    {

        public int Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }

        public float Balance { get; set; }

        public int NeededCash { get; set; }

        public int BetMultiplier { get; set; }

        public float AutoStopRatio { get; set; }

        public DateTime LastSpin { get; set; }
    }
}