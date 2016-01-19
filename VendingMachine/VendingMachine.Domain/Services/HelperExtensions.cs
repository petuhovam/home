using System;
using System.Collections.Generic;

using VendingMachine.Domain.Models;

namespace VendingMachine.Domain.Services
{
    public static class HelperExtensions
    {
        /// <summary>
        /// Копировать монету N раз
        /// </summary>
        public static IEnumerable<Money> Copy(this Money money, UInt32 n)
        {
            for (var i = 0; i < n; i++)
                yield return money;
        }

        /// <summary>
        /// Копировать товар N раз
        /// </summary>
        public static IEnumerable<Product> Copy(this Product product, UInt32 n)
        {
            if (product == null)
                yield break;

            for (var i = 0; i < n; i++)
                yield return product.Clone();
        }

        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                list.Add(item);
            }
        }
    }
}
