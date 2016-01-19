using System;

namespace VendingMachine.Domain.Services.Common
{
    public interface ILogsService
    {
        void Trace(String message);

        void Error(Exception ex);
    }
}
