using System;

namespace VendingMachine.Domain.Services.Common
{
    /// <summary>
    /// Сервис сообщений
    /// </summary>
    public interface IMsgService
    {
        void Show(String format, params Object[] args);

        void Error(String format, params Object[] args);
    }
}
