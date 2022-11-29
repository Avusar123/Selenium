using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Exceptions
{
    public class APIException : Exception
    {
        public APIException(string json) : base(json)
        {

        }
    }
}
