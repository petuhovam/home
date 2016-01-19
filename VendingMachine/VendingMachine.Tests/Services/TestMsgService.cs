using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using VendingMachine.Domain.Services.Common;

namespace VendingMachine.Tests.Services
{
    [Export(typeof(IMsgService))]
    class TestMsgService : IMsgService
    {
        public void Show(String format, params Object[] args)
        {
        }

        public void Error(String format, params Object[] args)
        {
        }
    }
}
