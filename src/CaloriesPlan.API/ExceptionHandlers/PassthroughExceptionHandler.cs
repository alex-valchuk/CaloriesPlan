using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace CaloriesPlan.API.ExceptionHandlers
{
    /// <summary>
    /// Forwards the exception down the stack, and lets the middleware do all the actual work
    /// </summary>
    public class PassthroughExceptionHandler : IExceptionHandler
    {
        public async Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                var info = ExceptionDispatchInfo.Capture(context.Exception);
                info.Throw();
            });
        }
    }
}