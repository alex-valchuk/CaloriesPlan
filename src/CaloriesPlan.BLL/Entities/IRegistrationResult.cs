using System.Collections.Generic;

namespace CaloriesPlan.BLL.Entities
{
    public interface IRegistrationResult
    {
        IEnumerable<string> Errors { get; }
        bool Succeeded { get; }
    }
}
