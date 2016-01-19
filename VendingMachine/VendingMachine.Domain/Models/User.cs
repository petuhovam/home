using System;
using System.Collections.Generic;

using VendingMachine.Domain.Services;

namespace VendingMachine.Domain.Models
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class User
    {
        #region ctor

        public User()
        {
            Account = new Account();
            Products = new List<Product>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Счет пользователя
        /// </summary>
        public Account Account 
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// Купленные товары
        /// </summary>
        public IList<Product> Products
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
            Account.Add(Money.One.Copy(10));
            Account.Add(Money.Two.Copy(30));
            Account.Add(Money.Five.Copy(20));
            Account.Add(Money.Ten.Copy(15));
        }

        #endregion
    }
}
