using System;

namespace CaloriesPlan.BLL.Exceptions
{
    public class InvalidPasswordConfirmationException : Exception
    {
        public string PropertyName { get; private set; }

        public InvalidPasswordConfirmationException(string propertyName, string message)
            : base(message)
        {
            this.PropertyName = propertyName;
        }
    }
}
