using System;

namespace CaloriesPlan.BLL.Exceptions
{
    public class InvalidDateRangeException : Exception
    {
        public InvalidDateRangeException(string message)
            : base(message)
        {
        }
    }
}
