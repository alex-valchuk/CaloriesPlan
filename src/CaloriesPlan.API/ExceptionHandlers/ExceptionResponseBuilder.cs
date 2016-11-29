using System;
using System.Net;

using Microsoft.Owin;

using CaloriesPlan.BLL.Exceptions;

namespace CaloriesPlan.API.ExceptionHandlers
{
    /// <summary>
    /// Constructs response for all application exceptions 
    /// </summary>
    public static class ExceptionResponseBuilder
    {
        public static void HandleException(IOwinContext context, Exception exception)
        {
            var httpStatusCode = HttpStatusCode.InternalServerError;

            if (exception is NotImplementedException)
            {
                httpStatusCode = HttpStatusCode.NotImplemented;
            }
            else if (exception is AccountDoesNotExistException)
            {
                httpStatusCode = HttpStatusCode.Unauthorized;
            }
            else if (exception is InvalidDateRangeException)
            {
                httpStatusCode = HttpStatusCode.BadRequest;
            }
            else if (exception is MealDoesNotExistException)
            {
                httpStatusCode = HttpStatusCode.NotFound;
            }

            context.Response.StatusCode = (int)httpStatusCode;
        }
    }
}