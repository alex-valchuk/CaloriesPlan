using System.Collections.Generic;

namespace CaloriesPlan.UTL.Wrappers
{
    public interface IAccountRegistrationResult
    {
        IEnumerable<string> Errors { get; }
        bool Succeeded { get; }
    }
}
