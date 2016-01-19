using System;

using VendingMachine.UI.WPF.Models;
using VendingMachine.Domain.Models;

namespace VendingMachine.UI.WPF.Commands
{
    public class PutMoneyCommand : Command<MoneyCount>
    {
        readonly Account _sourceAccount, _targetAccount;

        public PutMoneyCommand(Account source, Account target) 
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (target == null)
                throw new ArgumentNullException("target");

            _sourceAccount = source;
            _targetAccount = target;

            execute = p => PutMoney(p.Money);
            canExecute = p => CanPutMoney(p);
        }

        public Boolean CanPutMoney(MoneyCount item)
        {
            return item != null && item.Count > 0;
        }

        public void PutMoney(Money item)
        {
            var money = _sourceAccount.Get(item);
            _targetAccount.Add(money);
        }
    }
}
