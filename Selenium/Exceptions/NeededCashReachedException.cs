using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
