using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

using VendingMachine.Domain.Services.Common;
using VendingMachine.Domain.Services.Mef;

namespace VendingMachine.UI.AspNetMvc.Services
{
    class MefControllerFactory : DefaultControllerFactory
    {
        #region Methods

        private IServiceContainer CreateContainer(RequestContext requestContext)
        {
            var session = requestContext.HttpContext.Session;

            var container = session["MefContainer"] as IServiceContainer;
            if (container == null)
            {
                container = new MefServiceContainer(GetType().Assembly, typeof(MefServiceContainer).Assembly);
                container.RegisterInstance<ILogsService, ILogsService>(TraceLogsService.Default);
                
                var context = container.BuildUp(new MvcDomainModelContext());
                context.CreateDefaults();

                container.RegisterInstance<IDomainModelContext, MvcDomainModelContext>(context);

                session["MefContainer"] = container;                
            }
            return container;
        }

        public override IController CreateController(RequestContext requestContext, String controllerName)
        {
            var controller = base.CreateController(requestContext, controllerName);
            var container = CreateContainer(requestContext);
            return container.BuildUp(controller);
        }

        #endregion
    }
}