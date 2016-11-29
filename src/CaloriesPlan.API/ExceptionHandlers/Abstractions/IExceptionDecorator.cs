using System;
using System.Threading.Tasks;

using Microsoft.Owin;

namespace CaloriesPlan.API.ExceptionHandlers.Abstractions
{
    public interface IExceptionDecorator
    {
        Task DecorateRequest(IOwinContext ctx, Func<Task> task);
    }
}
