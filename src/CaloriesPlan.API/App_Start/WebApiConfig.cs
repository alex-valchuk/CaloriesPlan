using System.Linq;
using System.Web.Http;
using System.Net.Http.Formatting;
using System.Web.Http.ExceptionHandling;

using Microsoft.Owin.Security.OAuth;

using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

using CaloriesPlan.API.ExceptionHandlers;

namespace CaloriesPlan.API
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            //config.Filters.Add(new ApplicationExceptionFilterAttribute());// handles exceptions from controllers

            config.Services.Replace(typeof(IExceptionHandler), new PassthroughExceptionHandler());// handles all exceptions that were not intercepted

            //Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultRoute",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });


            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            var serializerSettings = jsonFormatter.SerializerSettings;
            
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            serializerSettings.Converters.Add(new IsoDateTimeConverter());
        }
    }
}