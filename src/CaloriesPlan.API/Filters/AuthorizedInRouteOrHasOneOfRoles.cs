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
    public class AuthorizedInRouteOrHasOneOfRoles : AuthorizeAttribute
    {
        private readonly string[] supportedRoles;

        public AuthorizedInRouteOrHasOneOfRoles(params string[] supportedRoles)
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

            var isOwner = false;

            var routeParams = actionContext.Request.GetRouteData().Values;
            if (routeParams.ContainsKey(AuthorizationParams.ParameterUserName))
            {
                var currentUserName = currentUser.Identity.Name;
                var requestUserName = (string)routeParams[AuthorizationParams.ParameterUserName];

                isOwner = (currentUserName.ToLower() == requestUserName.ToLower());
            }

            return isOwner;
        }
    }
}