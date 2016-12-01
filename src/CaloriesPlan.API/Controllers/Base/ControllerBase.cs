using System;
using System.Linq;
using System.Web.Http;

using CaloriesPlan.UTL.Const;
using CaloriesPlan.BLL.Exceptions;

namespace CaloriesPlan.API.Controllers.Base
{
    public abstract class ControllerBase : ApiController
    {
        protected const string ParamUserName = "{" + AuthorizationParams.ParameterUserName + "}";
        protected const string ParamRoleName = "{" + AuthorizationParams.ParameterRoleName + "}";
        protected const string ParamID = "{" + AuthorizationParams.ParameterID + "}";

        protected bool IsAuthorizedUserAnAdmin()
        {
            return this.User.IsInRole(AuthorizationParams.RoleAdmin);
        }
    }
}