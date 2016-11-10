using System.Collections.Generic;

using Microsoft.AspNet.Identity;

namespace CaloriesPlan.BLL.Entities.AspNetIdentity
{
    public class AspNetIdentityRegistrationResut : IRegistrationResult
    {
        private readonly IdentityResult identityResult;

        public AspNetIdentityRegistrationResut(IdentityResult identityResult)
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
