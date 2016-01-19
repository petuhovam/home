using System;

using VendingMachine.Domain.Models;
using VendingMachine.Domain.Services.Domain;
using VendingMachine.Domain.Services.Common;

using VendingMachine.UI.WPF.Models;

namespace VendingMachine.UI.WPF.Commands
{
    public class BuyProductCommand : Command<ProductCount>
    {
        readonly IVMService _vm;
        readonly User _user;
        readonly IMsgService _msg;

        public BuyProductCommand(IVMService vm, User user, IMsgService msg) 
        {
            if (vm == null)
                throw new ArgumentNullException("vm");
            if (user == null)
                throw new ArgumentNullException("user");
            if (msg == null)
                throw new ArgumentNullException("msg");

            _vm = vm;
            _user = user;
            _msg = msg;

            execute = p => BuyProduct(p.Product);
            canExecute = p => CanBuyProduct(p);
        }

        public Boolean CanBuyProduct(ProductCount item)
        {
            return item != null && item.Count > 0; // && item.Product.Price <= _vm.UserAccount.TotalSum;
        }

        public void BuyProduct(Product item)
        {
            _vm.Buy(item);

            _user.Products.Add(item);

            _msg.Show("Спасибо!");
        }
    }
}
