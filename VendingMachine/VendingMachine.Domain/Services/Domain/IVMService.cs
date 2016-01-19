using System;
using System.Collections.Generic;

using VendingMachine.Domain.Models;

namespace VendingMachine.Domain.Services.Domain
{
    /// <summary>
    /// Сервис работы торгового автомата
    /// </summary>
    public interface IVMService
    {
        /// <summary>
        /// Товары в наличии
        /// </summary>
        IList<Product> Products
        {
            get;
        }

        /// <summary>
        /// Счет автомата
        /// </summary>
        Account BankAccount
        {
            get;
        }

        /// <summary>
        /// Счет пользователя
        /// </summary>
        Account UserAccount
        {
            get;
        }

        /// <summary>
        /// Задать начальные значения
        /// </summary>
        void CreateDefaults();

        /// <summary>
        /// Купить товар
        /// </summary>
        Product Buy(String productName);

        /// <summary>
        /// Купить товар
        /// </summary>
        void Buy(Product product);

        /// <summary>
        /// Получить остаток
        /// </summary>
        Money[] GetRest();
    }
}
