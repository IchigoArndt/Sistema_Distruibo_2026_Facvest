using System;
using System.Collections.Generic;
using System.Text;

namespace SD_Api_Base.Exceptions
{
    public class SeedException(string message) : Exception(message)
    {
    }
}
