using System.Web.Http;

using CaloriesPlan.UTL.Const;

namespace CaloriesPlan.API.Controllers.Base
{
    public abstract class ControllerBase : ApiController
    {
        protected const string ParamUserName = "{" + AuthorizationParams.ParameterUserName + "}";
        protected const string ParamRoleName = "{" + AuthorizationParams.ParameterRoleName + "}";
        protected const string ParamID = "{" + AuthorizationParams.ParameterID + "}";
    }
}