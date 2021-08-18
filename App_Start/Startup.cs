using System;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Routing;
using ExploreCalifornia.Config;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;

[assembly: OwinStartup(typeof(ExploreCalifornia.Startup))]
namespace ExploreCalifornia
{
    public class Startup
    {
        public static HttpConfiguration HttpConfiguration { get; set; } = new HttpConfiguration();

        public void Configuration(IAppBuilder app)
        {
            var config = Startup.HttpConfiguration;
            ConfigureWebApi(app, config);
        }

        
        private static void ConfigureWebApi(IAppBuilder app, HttpConfiguration config)
        {
            // By default, Web API uses a JSON serializer when handling data.
            // XML can be enabled through the following line.
            // Now both JSON and XML can be returned, specified in the request through the Accept header.
            config.Formatters.XmlFormatter.UseXmlSerializer = true;

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            // Enable Web API route attributes
            config.MapHttpAttributeRoutes();

            // <ACTION> /api/<Name>Controller
            // GET /api/order -> OrderController.GetOrders()
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            app.UseWebApi(config);
        }
    }
}