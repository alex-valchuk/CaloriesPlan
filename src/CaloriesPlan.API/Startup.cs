using System;
using System.Web.Http;

using Microsoft.Practices.Unity;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

using CaloriesPlan.API.Providers;
using CaloriesPlan.BLL.Services;
using CaloriesPlan.BLL.Services.Impl;
using CaloriesPlan.DAL.Dao;
using CaloriesPlan.DAL.Dao.EF;
using CaloriesPlan.UTL;
using CaloriesPlan.UTL.Config.Desktop;

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
            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            var oAuthService = this.unityContainer.Resolve<IOAuthService>();

            var oAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/accounts/login"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new ApplicationOAuthProvider(oAuthService)
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        private void ConfigureDependencies(HttpConfiguration config)
        {
            this.unityContainer = new UnityContainer();
            
            //utils
            this.unityContainer.RegisterType<IConfigProvider, DesktopConfigProvider>(new HierarchicalLifetimeManager());

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