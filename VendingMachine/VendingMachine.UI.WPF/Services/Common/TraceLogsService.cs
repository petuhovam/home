using System;
using System.ComponentModel.Composition;

using VendingMachine.Domain.Services.Common;

namespace VendingMachine.UI.WPF.Services.Common
{
    [PartCreationPolicy(CreationPolicy.Shared)]
    [Export(typeof(ILogsService))]
    class TraceLogsService : ILogsService
    {
        public void Trace(String message)
        {
            System.Diagnostics.Trace.TraceInformation(message);
        }

        public void Error(Exception ex)
        {
            System.Diagnostics.Trace.TraceError(ex.ToString());
        }
    }
}
