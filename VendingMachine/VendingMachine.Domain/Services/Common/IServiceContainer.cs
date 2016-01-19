using System;

namespace VendingMachine.Domain.Services.Common
{
    /// <summary>
    /// Контейнер
    /// </summary>
    public interface IServiceContainer
    {
        void RegisterInstance<TService, TServiceInstance>(TServiceInstance serviceInstance) 
            where TServiceInstance : TService;

        TService Resolve<TService>();

        TServiceInstance BuildUp<TServiceInstance>(TServiceInstance serviceInstance);
    }
}
