using System;
using System.Windows.Input;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;

using VendingMachine.Domain.Models;
using VendingMachine.Domain.Services;
using VendingMachine.Domain.Services.Domain;

using VendingMachine.UI.WPF.Models;
using VendingMachine.UI.WPF.Commands;

namespace VendingMachine.UI.WPF.ViewModels
{
    [Export]
    public class MainViewModel : ViewModel
    {
        #region ctor

        public MainViewModel()
        {
            User = new User();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Пользователь
        /// </summary>
        public User User
        {
            get;
            private set;
        }

        /// <summary>
        /// Кошелек пользователя
        /// </summary>
        public AccountModel UserAccount
        {
            get { return Get(() => UserAccount, () => new AccountModel(User.Account)); }
        }

        /// <summary>
        /// Купленные товары
        /// </summary>
        public ProductModel UserProducts
        {
            get { return Get(() => UserProducts, () => new ProductModel(User.Products)); }
        }

        /// <summary>
        /// Товары в торговом автомате
        /// </summary>
        public ProductModel VMProducts
        {
            get { return Get(() => VMProducts, () => new ProductModel(VM.Products)); }
        }

        /// <summary>
        /// Кошелек для сдачи
        /// </summary>
        public AccountModel VMBankAccount
        {
            get { return Get(() => VMBankAccount, () => new AccountModel(VM.BankAccount)); }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Положить деньги на счет
        /// </summary>
        public ICommand OnPutMoney
        {
            get {
                return Get(
                    () => OnPutMoney,
                    () => GetProxyCommand(new PutMoneyCommand(User.Account, VM.UserAccount)));
            }
        }

        /// <summary>
        /// Купить товар
        /// </summary>
        public ICommand OnBuyProduct
        {
            get
            {
                return Get(
                    () => OnBuyProduct,
                    () => GetProxyCommand(new BuyProductCommand(VM, User, Msgs)));
            }
        }

        /// <summary>
        /// Получить сдачу
        /// </summary>
        public ICommand OnGetRest
        {
            get
            {
                return Get(
                    () => OnGetRest,
                    () => GetProxyCommand(new GetRestCommand(VM, User.Account)));
            }
        }

        #endregion

        #region Services

        [Import]
        public IVMService VM
        {
            get;
            private set;
        }

        #endregion

        #region Methods

        public void Build()
        {
            User.CreateDefaults();
            VM.CreateDefaults();

            UserAccount.Refresh();
            UserProducts.Refresh();
            VMProducts.Refresh();
            VMBankAccount.Refresh();
        }

        protected override void ExecuteProxyCommand(ICommand command, Object p)
        {
            base.ExecuteProxyCommand(command, p);

            UserAccount.Refresh();
            UserProducts.Refresh();

            VMProducts.Refresh();
            VMBankAccount.Refresh();
        }

        #endregion
    }
}
