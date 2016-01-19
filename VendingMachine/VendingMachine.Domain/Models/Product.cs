using System;
using System.Collections.Generic;

namespace VendingMachine.Domain.Models
{
    /// <summary>
    /// Товар
    /// </summary>
    public class Product : NotifyItem, IEquatable<String>, ICloneable
    {
        #region Members

        /// <summary>
        /// Чай
        /// </summary>
        public static readonly Product Tea = new Product("Чай");

        /// <summary>
        /// Кофе
        /// </summary>
        public static readonly Product Coffee = new Product("Кофе");

        /// <summary>
        /// Кофе с молоком
        /// </summary>
        public static readonly Product CoffeeWithMilk = new Product("Кофе с молоком");

        /// <summary>
        /// Сок
        /// </summary>
        public static readonly Product Juice = new Product("Сок");

        #endregion

        #region ctor

        public Product(String name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            Name = name;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Название
        /// </summary>
        public String Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Стоимость
        /// </summary>
        public Money Price
        {
            get;
            private set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Изменить стоимость товара
        /// </summary>
        public Product ChangePrice(Money price)
        {
            Price = price;
            Notify(() => Price);

            return this;
        }

        public override String ToString()
        {
            return String.Format("{0} за {1}", Name, Price);
        }

        #endregion

        #region IEquatable<String> Members

        public Boolean Equals(String other)
        {
            return String.Equals(Name, other, StringComparison.InvariantCultureIgnoreCase);
        }

        #endregion

        #region ICloneable

        public Product Clone()
        {
            return new Product(this.Name) { Price = this.Price };
        }

        Object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion
    }
}
