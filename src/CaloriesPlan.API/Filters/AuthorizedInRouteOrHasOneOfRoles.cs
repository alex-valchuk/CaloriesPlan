using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;

using CaloriesPlan.API.Filters.Base;

namespace CaloriesPlan.API.Filters
{
    public class AuthorizedInRouteOrHasOneOfRoles : AuthorizedInQueryOrHasOneOfRoles
    {
        public AuthorizedInRouteOrHasOneOfRoles(params string[] supportedRoles)
            : base(authorizeIfParameterNotDefined: false, supportedRoles: supportedRoles)
        {
        }

        protected override IDictionary<string, object> GetActionParameters(HttpActionContext actionContext)
        {
            return actionContext.Request.GetRouteData().Values;
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