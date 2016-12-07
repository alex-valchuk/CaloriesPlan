using System;
using System.Threading.Tasks;

using Microsoft.Owin.Security.OAuth;

using CaloriesPlan.BLL.Services.Abstractions;
using CaloriesPlan.UTL.Loggers.Abstractions;

namespace CaloriesPlan.API.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly IApplicationLogger applicationLogger;
        private readonly IOAuthService oAuthService;

        public ApplicationOAuthProvider(IApplicationLogger applicationLogger, IOAuthService oAuthService)
        {
            this.applicationLogger = applicationLogger;
            this.oAuthService = oAuthService;
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            try
            {
                // Resource owner password credentials does not provide a client ID.
                if (context.ClientId == null)
                {
                    context.Validated();
                }
            }
            catch (Exception ex)
            {
                this.applicationLogger.Error(ex);
            }

            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

                var authTicket = await this.oAuthService.SignInAsync(context.UserName, context.Password, context.Options.AuthenticationType);
                if (authTicket == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                }
                else
                {
                    // generating the token behind the scenes
                    context.Validated(authTicket);
                }
            }
            catch (Exception ex)
            {
                this.applicationLogger.Error(ex);
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            try
            {
                foreach (var property in context.Properties.Dictionary)
                {
                    context.AdditionalResponseParameters.Add(property.Key, property.Value);
                }
            }
            catch (Exception ex)
            {
                this.applicationLogger.Error(ex);
            }

            return Task.FromResult<object>(null);
        }
    }
}