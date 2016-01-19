using System;
using System.Linq;
using System.Reflection;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

using VendingMachine.Domain.Services.Common;

namespace VendingMachine.Domain.Services.Mef
{
    [PartCreationPolicy(CreationPolicy.Shared)]
    [Export(typeof(IServiceContainer))]
    public class MefServiceContainer : IServiceContainer
    {
        #region Members

        public readonly CompositionContainer Container;

        #endregion

        #region ctor

        public MefServiceContainer()
            : this(Assembly.GetExecutingAssembly())
        {
        }
        public MefServiceContainer(params Assembly[] assemblies)
        {
            var acs = assemblies.Select(a => new AssemblyCatalog(a));

            var catalog = new AggregateCatalog(acs);

            Container = new CompositionContainer(catalog);
        }

        #endregion

        #region IServiceContainer Members

        public void RegisterInstance<TService, TServiceInstance>(TServiceInstance serviceInstance)
            where TServiceInstance : TService
        {
            Container.ComposeExportedValue<TService>(serviceInstance);
        }

        public TService Resolve<TService>()
        {
            var serviceInstance = Container.GetExportedValue<TService>();
            return serviceInstance;
        }

        public TServiceInstance BuildUp<TServiceInstance>(TServiceInstance serviceInstance)
        {
            Container.ComposeParts(serviceInstance);
            return serviceInstance;
        }

        #endregion
    }
}
