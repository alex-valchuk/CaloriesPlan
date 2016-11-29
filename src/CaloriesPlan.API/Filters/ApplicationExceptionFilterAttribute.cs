using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

using CaloriesPlan.BLL.Exceptions;

namespace CaloriesPlan.API.Filters
{
    /// <summary>
    /// Constructs HttpResponse for particular exceptions
    /// </summary>
    public class ApplicationExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var exception = context.Exception;

            if (exception is NotImplementedException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
            }
            else if (exception is AccountDoesNotExistException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
            else if (exception is InvalidDateRangeException)
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, exception.Message);
            }
            else if (exception is MealDoesNotExistException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            else
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}