using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

using VendingMachine.Domain.Services.Mef;
using VendingMachine.Domain.Services.Common;

using VendingMachine.UI.AspNetMvc.Services;

namespace VendingMachine.UI.AspNetMvc
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
            routes.MapRoute("Error", "Home/Error");
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            ControllerBuilder.Current.SetControllerFactory(new MefControllerFactory());
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            Server.ClearError();

            TraceLogsService.Default.Error(exception);

            Response.RedirectToRoute("Error", new { message = exception.Message });            
        }
    }
}