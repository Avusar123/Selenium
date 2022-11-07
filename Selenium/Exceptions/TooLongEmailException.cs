using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
