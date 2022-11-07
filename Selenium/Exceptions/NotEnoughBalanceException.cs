using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
