using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium
{
    public class OneClickRegistrationResponce
    {
        public string Token { get; set; }

        public string Login { get; set; }

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
