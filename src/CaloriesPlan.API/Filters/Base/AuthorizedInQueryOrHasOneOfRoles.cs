using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

using CaloriesPlan.UTL.Const;

namespace CaloriesPlan.API.Filters.Base
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public abstract class AuthorizedInQueryOrHasOneOfRoles : AuthorizeAttribute
    {
        private readonly bool authorizeIfParameterNotDefined;
        private readonly string[] supportedRoles;

        public AuthorizedInQueryOrHasOneOfRoles(bool authorizeIfParameterNotDefined, params string[] supportedRoles)
        {
            this.authorizeIfParameterNotDefined = authorizeIfParameterNotDefined;
            this.supportedRoles = supportedRoles;
        }

        protected abstract IDictionary<string, object> GetActionParameters(HttpActionContext actionContext);

        protected bool Authorized(HttpActionContext actionContext)
        {
            var currentUser = HttpContext.Current.User;

            var isCurrentUserHasSupportedRole = this.IsCurrentUserHasSupportedRole(currentUser);
            if (isCurrentUserHasSupportedRole)
                return true;

            var isCurrentUserIsInRequestedParameter = this.IsCurrentUserIsInRequestedParameter(actionContext, currentUser);
            return isCurrentUserIsInRequestedParameter;
        }

        private bool IsCurrentUserHasSupportedRole(IPrincipal currentUser)
        {
            foreach (var supportedRole in this.supportedRoles)
            {
                if (currentUser.IsInRole(supportedRole))
                    return true;
            }

            return false;
        }

        private bool IsCurrentUserIsInRequestedParameter(HttpActionContext actionContext, IPrincipal currentUser)
        {
            var isOwner = this.authorizeIfParameterNotDefined;

            var actionParams = this.GetActionParameters(actionContext);
            if (actionParams.ContainsKey(AuthorizationParams.ParameterUserName))
            {
                var currentUserName = currentUser.Identity.Name;
                var requestUserName = (string)actionParams[AuthorizationParams.ParameterUserName];

                isOwner = (currentUserName.ToLower() == requestUserName.ToLower());
            }

            return isOwner;
        }
    }
}