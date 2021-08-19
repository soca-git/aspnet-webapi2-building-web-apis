
using System.Web.Mvc;

namespace ExploreCalifornia
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Register HelpPages areas.
            AreaRegistration.RegisterAllAreas();
        }
    }
}
