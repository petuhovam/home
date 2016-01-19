using System;
using System.ComponentModel.Composition;

using VendingMachine.Domain.Services.Common;

namespace VendingMachine.Tests.Services
{
    [Export(typeof(ILogsService))]
    class TestLogsService : ILogsService
    {
        public void Trace(String message)
        {
        }

        public void Error(Exception ex)
        {
        }
    }
}
