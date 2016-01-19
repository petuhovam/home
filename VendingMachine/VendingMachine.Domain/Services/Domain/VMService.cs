using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;

using VendingMachine.Domain.Models;
using VendingMachine.Domain.Erros;
using VendingMachine.Domain.Services.Domain;
using VendingMachine.Domain.Services.Common;

namespace VendingMachine.Domain.Services
{
    [Export(typeof(IVMService))]
    class VMService : IVMService
    {
        #region ctor

        public VMService()
        {
            BankAccount = new Account();
            UserAccount = new Account();
            Products = new List<Product>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Товары в наличии
        /// </summary>
        public IList<Product> Products
        {
            get;
            private set;
        }

        /// <summary>
        /// Счет автомата
        /// </summary>
        public Account BankAccount
        {
            get;
            private set;
        }

        /// <summary>
        /// Счет пользователя
        /// </summary>
        public Account UserAccount
        {
            get;
            private set;
        }

        #endregion

        #region Services

        [Import]
        public ILogsService Logs
        {
            get;
            private set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Задать начальные значения
        /// </summary>
        public void CreateDefaults()
        {
            Logs.Trace("Задать начальные значения");

            BankAccount.Add(Money.One.Copy(100));
            BankAccount.Add(Money.Two.Copy(100));
            BankAccount.Add(Money.Five.Copy(100));
            BankAccount.Add(Money.Ten.Copy(100));

            Products.AddRange(Product.Tea.ChangePrice(new Money(13)).Copy(10));
            Products.AddRange(Product.Coffee.ChangePrice(new Money(18)).Copy(20));
            Products.AddRange(Product.CoffeeWithMilk.ChangePrice(new Money(21)).Copy(20));
            Products.AddRange(Product.Juice.ChangePrice(new Money(35)).Copy(15));
        }

        /// <summary>
        /// Купить
        /// </summary>
        public Product Buy(String productName)
        {
            if (productName == null)
                throw new ArgumentNullException("productName");

            var product = Products
                .Where(p => p.Equals(productName))
                .FirstOrDefault();

            if (product == null)
                throw VMException.ProductNotFound;

            Buy(product);

            return product;
        }

        /// <summary>
        /// Купить
        /// </summary>
        public void Buy(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            Logs.Trace("Купить: " + product);

            if (product.Price > UserAccount.TotalSum)
                throw VMException.InsufficientFunds;

            if (Products.Remove(product))
            {
                BankAccount.Plus(product.Price);
                UserAccount.Minus(product.Price);

                if (UserAccount.TotalSum == Money.Zero)
                {
                    GetRest();
                }

                Logs.Trace("Покупка прошла успешно: " + product);
            }
            else
                throw VMException.ProductNotFound;
        }

        /// <summary>
        /// Получить остаток
        /// </summary>
        public Money[] GetRest()
        {
            Logs.Trace("Получить остаток: " + UserAccount);

            var rest = UserAccount.GetAll(BankAccount);
            if (rest.Length > 0)
            {
                Logs.Trace("Остаток успешно получен");
            }
            return rest;
        }

        #endregion
    }
}
