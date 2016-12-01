using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;

using CaloriesPlan.API.Filters.Base;

namespace CaloriesPlan.API.Filters
{
    public class AuthorizedInParamOrHasOneOfRoles : AuthorizedInQueryOrHasOneOfRoles
    {
        public AuthorizedInParamOrHasOneOfRoles(params string[] supportedRoles)
            : base(authorizeIfParameterNotDefined: true, supportedRoles: supportedRoles)
        {
        }

        protected override IDictionary<string, object> GetActionParameters(HttpActionContext actionContext)
        {
            return actionContext.Request.GetQueryNameValuePairs().ToDictionary(p => p.Key, x => (object)x.Value);
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (!this.Authorized(actionContext))
            {
                this.HandleUnauthorizedRequest(actionContext);
            }
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
        }
    }
}