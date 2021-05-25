using Autofac;

using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Threading.Tasks;

using WorkAutomatorLogic;
using WorkAutomatorLogic.ServiceInterfaces;

namespace WorkAutomatorServer
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }
    }
}
