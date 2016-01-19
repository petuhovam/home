using System;

using VendingMachine.Domain.Models;
using VendingMachine.Domain.Services.Domain;

using VendingMachine.UI.WPF.Models;

namespace VendingMachine.UI.WPF.Commands
{
    public class GetRestCommand : Command<Object>
    {
        readonly IVMService _vm;
        readonly Account _targetAccount;

        public GetRestCommand(IVMService vm, Account targetAccount) 
        {
            if (vm == null)
                throw new ArgumentNullException("vm");
            if (targetAccount == null)
                throw new ArgumentNullException("targetAccount");

            _vm = vm;
            _targetAccount = targetAccount;

            execute = p => GetRest();
            canExecute = p => CanGetRest();
        }

        public Boolean CanGetRest()
        {
            return _vm.UserAccount.TotalSum != Money.Zero;
        }

        public void GetRest()
        {
            var rest = _vm.GetRest();
            if (rest.Length > 0)
                _targetAccount.Add(rest);
        }
    }
}
