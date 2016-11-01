using System.Threading.Tasks;

using Microsoft.Owin.Security.OAuth;

using CaloriesPlan.BLL.Services;

namespace CaloriesPlan.API.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly IOAuthService oAuthService;

        public ApplicationOAuthProvider(IOAuthService oAuthService)
        {
            this.oAuthService = oAuthService;
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            
            var authTicket = await this.oAuthService.GetAuthenticationTicket(context.UserName, context.Password, context.Options.AuthenticationType);
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

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}