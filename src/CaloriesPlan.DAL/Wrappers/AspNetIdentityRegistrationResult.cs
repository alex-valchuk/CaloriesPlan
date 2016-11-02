using System.Collections.Generic;

using Microsoft.AspNet.Identity;

using CaloriesPlan.UTL.Wrappers;

namespace CaloriesPlan.DAL.Wrappers
{
    public class AspNetIdentityRegistrationResult : IAccountRegistrationResult
    {
        private readonly IdentityResult identityResult;

        public AspNetIdentityRegistrationResult(IdentityResult identityResult)
        {
            this.identityResult = identityResult;
        }

        public IEnumerable<string> Errors
        {
            get
            {
                if (this.identityResult != null)
                {
                    return this.identityResult.Errors;
                }

                return null;
            }
        }

        public bool Succeeded
        {
            get
            {
                return 
                    this.identityResult != null &&
                    this.identityResult.Succeeded;
            }
        }
    }
}
