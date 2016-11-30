using System;
using System.Threading.Tasks;

using Microsoft.Owin;

using CaloriesPlan.UTL.Loggers.Abstractions;
using CaloriesPlan.API.ExceptionHandlers.Abstractions;

namespace CaloriesPlan.API.ExceptionHandlers
{
    public class GlobalExceptionDecorator : IExceptionDecorator
    {
        private readonly IApplicationLogger logger;

        public GlobalExceptionDecorator(IApplicationLogger logger)
        {
            this.logger = logger;
        }

        public async Task DecorateRequest(IOwinContext ctx, Func<Task> task)
        {
            try
            {
                await task();
            }
            catch (Exception ex)
            {
                this.logger.Error(ex);

                ExceptionResponseBuilder.HandleException(ctx, ex);
            }
        }
    }
}