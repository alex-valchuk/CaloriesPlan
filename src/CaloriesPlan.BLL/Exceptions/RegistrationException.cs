using System;

using CaloriesPlan.UTL.Wrappers;

namespace CaloriesPlan.BLL.Exceptions
{
    public class RegistrationException : Exception
    {
        public IAccountRegistrationResult RegistrationResult { get; private set; }

        public RegistrationException(IAccountRegistrationResult registrationResult)
        {
            this.RegistrationResult = registrationResult;
        }

        public RegistrationException(string message, IAccountRegistrationResult registrationResult)
            : base(message)
        {
            this.RegistrationResult = registrationResult;
        }
    }
}
