using System;
using System.Web.Http;

using Microsoft.Practices.Unity;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;

using Owin;

using CaloriesPlan.BLL.Services;
using CaloriesPlan.BLL.Services.Impl;
using CaloriesPlan.DAL.Dao;
using CaloriesPlan.DAL.Dao.EF;
using CaloriesPlan.UTL;
using CaloriesPlan.UTL.Config.Desktop;
using CaloriesPlan.UTL.Loggers.Abstractions;
using CaloriesPlan.UTL.Loggers;
using CaloriesPlan.API.Providers;
using CaloriesPlan.API.ExceptionHandlers;
using CaloriesPlan.API.ExceptionHandlers.Abstractions;

[assembly: OwinStartup(typeof(CaloriesPlan.API.Startup))]
namespace CaloriesPlan.API
{
    public class Startup
    {
        private IUnityContainer unityContainer;

        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll); //allows cross domain requests

            HttpConfiguration config = new HttpConfiguration();
            
            this.ConfigureDependencies(config);
            this.ConfigureOAuth(app);

            WebApiConfig.Register(config);

            var exceptionDecorator = this.unityContainer.Resolve<IExceptionDecorator>();

            app
                //global exception handling
                .Use(async (ctx, next) => await exceptionDecorator.DecorateRequest(ctx, next))
                .UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            var applicationLogger = this.unityContainer.Resolve<IApplicationLogger>();
            var oAuthService = this.unityContainer.Resolve<IOAuthService>();

            var oAuthServerOptions = new OAuthAuthorizationServerOptions
            {
#if DEBUG
                AllowInsecureHttp = true,
#endif
                TokenEndpointPath = new PathString("/api/accounts/signin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new ApplicationOAuthProvider(applicationLogger, oAuthService)
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }


        private void ConfigureDependencies(HttpConfiguration config)
        {
            this.unityContainer = new UnityContainer();

            //exception handlers
            this.unityContainer.RegisterType<IExceptionDecorator, GlobalExceptionDecorator>(new HierarchicalLifetimeManager());

            //utils
            this.unityContainer.RegisterType<IConfigProvider, DesktopConfigProvider>(new HierarchicalLifetimeManager());
            this.unityContainer.RegisterType<IApplicationLogger, NLogger>(new HierarchicalLifetimeManager());

            //dao
            this.unityContainer.RegisterType<IMealDao, EFMealDao>(new HierarchicalLifetimeManager());
            this.unityContainer.RegisterType<IUserDao, EFUserDao>(new HierarchicalLifetimeManager());

            //services
            this.unityContainer.RegisterType<IAccountService, AccountService>(new HierarchicalLifetimeManager());
            this.unityContainer.RegisterType<IOAuthService, AccountService>(new HierarchicalLifetimeManager());
            this.unityContainer.RegisterType<IMealService, MealService>(new HierarchicalLifetimeManager());

            config.DependencyResolver = new UnityDependencyResolver(this.unityContainer);
        }
    }
}