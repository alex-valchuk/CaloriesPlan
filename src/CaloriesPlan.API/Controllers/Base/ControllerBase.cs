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

        protected IHttpActionResult BadRegistration(RegistrationException exception)
        {
            if (exception != null &&
                exception.RegistrationResult != null)
            {
                var errors = exception.RegistrationResult.Errors;
                if (errors != null &&
                    errors.Count() > 0)
                {
                    foreach (string error in errors)
                    {
                        this.ModelState.AddModelError("", error);
                    }

                    return this.BadRequest(this.ModelState);
                }
            }

            return this.BadRequest();
        }

        protected IHttpActionResult BadProperty(InvalidPasswordConfirmationException exception)
        {
            if (exception != null)
            {
                this.ModelState.AddModelError(exception.PropertyName, exception.Message);
                return this.BadRequest(this.ModelState);
            }

            return this.BadRequest();
        }

        protected IHttpActionResult BadArgument(ArgumentNullException exception)
        {
            if (exception != null)
            {
                this.ModelState.AddModelError(exception.ParamName, exception.Message);
                return this.BadRequest(this.ModelState);
            }

            return this.BadRequest();
        }
    }
}