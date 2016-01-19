using System;
using System.Collections.Generic;

namespace VendingMachine.Domain.Models
{
    /// <summary>
    /// Деньги (монета)
    /// </summary>
    public struct Money : IEquatable<Money>, IComparable<Money>
    {
        #region Members

        const UInt16 MinValue = 0;

        const UInt16 MaxValue = 10000;

        /// <summary>
        /// Ноль
        /// </summary>
        public static readonly Money Zero = 0;

        /// <summary>
        /// Максимально возможная сумма
        /// </summary>
        public static readonly Money Limit = MaxValue;

        /// <summary>
        /// Один
        /// </summary>
        public static readonly Money One = 1;

        /// <summary>
        /// Два
        /// </summary>
        public static readonly Money Two = 2;

        /// <summary>
        /// Пять
        /// </summary>
        public static readonly Money Five = 5;

        /// <summary>
        /// Десять
        /// </summary>
        public static readonly Money Ten = 10;

        /// <summary>
        /// Номинал
        /// </summary>
        private readonly UInt16 Value;

        #endregion

        #region ctor

        public Money(UInt16 val)
        {
            if (val < MinValue || val > MaxValue)
                throw new ArgumentOutOfRangeException("val");

            Value = val;
        }

        #endregion

        #region Methods

        public override Boolean Equals(Object obj)
        {
            return Equals((Money)obj);
        }

        public override Int32 GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override String ToString()
        {
            return String.Format("{0} руб", Value);
        }

        #endregion

        #region IEquatable<Money> Members

        public Boolean Equals(Money other)
        {
            return Value.Equals(other.Value);
        }

        #endregion

        #region IComparable<Money> Members

        public Int32 CompareTo(Money other)
        {
            return Value.CompareTo(other.Value);
        }

        #endregion

        #region Operators

        public static implicit operator Money(UInt16 v)  
        {
            return new Money(v); 
        }

        public static explicit operator UInt16(Money m)
        {
            return m.Value;
        }

        public static Boolean operator ==(Money m1, Money m2)
        {
            return m1.Equals(m2);
        }

        public static Boolean operator !=(Money m1, Money m2)
        {
            return !m1.Equals(m2);
        }

        public static Boolean operator >(Money m1, Money m2)
        {
            return m1.CompareTo(m2) > 0;
        }

        public static Boolean operator >=(Money m1, Money m2)
        {
            return m1.CompareTo(m2) >= 0;
        }

        public static Boolean operator <(Money m1, Money m2)
        {
            return m1.CompareTo(m2) < 0;
        }

        public static Boolean operator <=(Money m1, Money m2)
        {
            return m1.CompareTo(m2) <= 0;
        }

        public static Money operator +(Money m1, Money m2)
        {
            var val = (UInt16)(m1.Value + m2.Value);
            return new Money(val);
        }

        public static Money operator -(Money m1, Money m2)
        {
            var val = (UInt16)(m1.Value - m2.Value);
            return new Money(val);
        }

        #endregion
    }
}
