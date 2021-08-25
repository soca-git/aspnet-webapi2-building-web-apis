using System;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Routing;
using ExploreCalifornia.Config;
using ExploreCalifornia.ExceptionHandlers;
using ExploreCalifornia.Filters;
using ExploreCalifornia.Loggers;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using Swashbuckle.Application;


// OWIN Startup class.
// This class is used to configure various application settings.
[assembly: OwinStartup(typeof(ExploreCalifornia.Startup))]
namespace ExploreCalifornia
{
    public class Startup
    {
        public static HttpConfiguration HttpConfiguration { get; set; } = new HttpConfiguration();

        public void Configuration(IAppBuilder app)
        {
            var config = Startup.HttpConfiguration;

            // "Message": "An error has occurred.",
            // "ExceptionMessage": "Self referencing loop detected with type 'ExploreCalifornia.Data.Entities.Reservation'. Path '[0].Tour.Reservations'.",
            // Ignore (set repeated objects to null) reference loops instead of throwing error.
            var jsonSettings = config.Formatters.JsonFormatter.SerializerSettings;
            jsonSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            ConfigureWebApi(app, config);

            // Added Swagger to Web API.
            // http://localhost:52201/swagger/ui/index
            ConfigureSwashbuckle(config);

            // Added JSON Web Tokens to Web API.
            ConfigureJwt(app);
        }
        
        // Configure Web API settings.
        private static void ConfigureWebApi(IAppBuilder app, HttpConfiguration config)
        {
            // Replace the default ExceptionLogger class with the newly created one.
            config.Services.Replace(typeof(IExceptionLogger), new UnhandledExceptionLogger());

            // Replace the default ExceptionHandler class with the newly created one.
            config.Services.Replace(typeof(IExceptionHandler), new UnhandledExceptionHandler());

            // Register a global exception filter, so that in runs on all controllers.
            config.Filters.Add(new DbUpdateExceptionFilterAttribute());

            // By default, Web API uses a JSON serializer when handling data.
            // XML can be enabled through the following line.
            // Now both JSON and XML can be returned, specified in the request through the Accept header.
            config.Formatters.XmlFormatter.UseXmlSerializer = true;

            // Add the AutoAuthentication handler to message handlers to enable request interception on Authorize attribute endpoints.
            // This is a quick way to authenticate all requests by creating dummy User identities.
            //config.MessageHandlers.Add(new AutoAuthenticationHandler());

            // Add the JWT handler to message handlers to enable request interception on Authorize attribute endpoints.
            // This means that securing endpoints with JWT is as easy as adding the Authorize attribute to the endpoint!
            config.MessageHandlers.Add(new TokenValidationHandler());

            // Enable Web API route attributes.
            config.MapHttpAttributeRoutes();

            // Default routing rule.
            // <ACTION> /api/<Name>Controller/<optional_id>
            // GET /api/order -> OrderController.GetOrders()
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/v2/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                // Added constraint to match id to any sequence of integers.
                //constraints: new { id =@"\d+" } 
                // (removed in favour of route attributes).
            );

            // Allow cross domain calls.
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            // Additional routing rule added for non-integers.
            // (removed in favour of route attributes).
            //config.Routes.MapHttpRoute(
            //    name: "DefaultNameApi",
            //    routeTemplate: "api/{controller}/{name}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            app.UseWebApi(config);
        }

        // Configure Swagger settings.
        private static void ConfigureSwashbuckle(HttpConfiguration config)
        {
            config.EnableSwagger(c => {
                // Customize API version & title.
                c.SingleApiVersion("v2", "WebApiV2");
                // Include auto-generated XML comments from bin/ExploreCalifornia.xml
                c.IncludeXmlComments($"{AppDomain.CurrentDomain.BaseDirectory}\\bin\\ExploreCalifornia.xml");
            }).EnableSwaggerUi();
        }

        // Configure JWT settings.
        private static void ConfigureJwt(IAppBuilder app)
        {
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    // Allowed audience "explore-cali-audience".
                    AllowedAudiences = new[] { GlobalConfig.Audience },
                    // Issuer "explore-cali-issuer" with Secret key "1a13faeb7d22432f808bacfea5c0f8fc58c9ff6ff14b46108df06960e4a3c1f9".
                    IssuerSecurityKeyProviders = new IIssuerSecurityKeyProvider[]
                    {
                        new SymmetricKeyIssuerSecurityKeyProvider(GlobalConfig.Issuer, GlobalConfig.Secret)
                    }
                });
        }
    }
}
