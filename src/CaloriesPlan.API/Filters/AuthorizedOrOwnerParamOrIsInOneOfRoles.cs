using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

using CaloriesPlan.UTL.Const;

namespace CaloriesPlan.API.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AuthorizedOrOwnerParamOrIsInOneOfRoles : AuthorizeAttribute
    {
        private readonly string[] supportedRoles;

        public AuthorizedOrOwnerParamOrIsInOneOfRoles(params string[] supportedRoles)
        {
            this.supportedRoles = supportedRoles;
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

        private bool Authorized(HttpActionContext actionContext)
        {
            var currentUser = HttpContext.Current.User;
            foreach (var supportedRole in this.supportedRoles)
            {
                if (currentUser.IsInRole(supportedRole))
                    return true;
            }

            var isAllowed = false;

            var queryParams = actionContext.Request.GetQueryNameValuePairs();
            if (queryParams != null &&
                queryParams.Any(p => p.Key.ToLower() == AuthorizationParams.ParameterUserName.ToLower()))
            {
                var currentUserName = currentUser.Identity.Name;
                var requestUserName = queryParams.First(p => p.Key.ToLower() == AuthorizationParams.ParameterUserName.ToLower()).Value;

                isAllowed = (currentUserName.ToLower() == requestUserName.ToLower());
            }
            else
            {
                isAllowed = true;
            }


            return isAllowed;
        }
    }
}