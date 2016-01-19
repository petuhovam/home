using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using VendingMachine.Domain.Erros;

namespace VendingMachine.Domain.Models
{
    /// <summary>
    /// Счет (кошелек)
    /// </summary>
    public class Account : NotifyItem, ICloneable, IEnumerable<Money>
    {
        #region Members

        readonly List<Money> _list = new List<Money>();

        private Money _plus = Money.Zero;
        private Money _minus = Money.Zero;

        #endregion

        #region Properties

        /// <summary>
        /// Сумма наличных денег
        /// </summary>
        public Money Sum
        {
            get;
            private set;
        }

        /// <summary>
        /// Общая сумма средств
        /// </summary>
        public Money TotalSum
        {
            get { return Sum + _plus - _minus; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Добавить деньги на счет
        /// </summary>
        public Account Add(IEnumerable<Money> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            return Add(items.ToArray());
        }
        /// <summary>
        /// Добавить деньги на счет
        /// </summary>
        public Account Add(params Money[] items)
        {
            _list.AddRange(items);

            Calculate();

            return this;
        }

        /// <summary>
        /// Удалить со счета
        /// </summary>
        public Account Remove(Money item)
        {
            _list.Remove(item);

            Calculate();

            return this;
        }

        /// <summary>
        /// Очистить счет
        /// </summary>
        public Account Clear()
        {
            _list.Clear();

            Reset();
            Calculate();

            return this;
        }

        /// <summary>
        /// Зачислить сумму
        /// </summary>
        public void Plus(Money amount)
        {
            if (TotalSum > Money.Limit - amount)
                throw new VMException("Невозможно зачислить деньги: превышение лимита");

            _plus += amount;

            Notify(() => Sum);
            Notify(() => TotalSum);
        }

        /// <summary>
        /// Списать сумму
        /// </summary>
        public void Minus(Money amount)
        {
            if (amount > TotalSum)
                throw new VMException("Новозможно списать деньги: надостаточно средств");

            _minus += amount;

            Notify(() => Sum);
            Notify(() => TotalSum);
        }

        /// <summary>
        /// Получить деньги и удалить со счета
        /// </summary>
        public Money[] Get(params Money[] money)
        {
            var list = new List<Money>();

            foreach (var m in money)
            {
                var item = _list.Where(i => i == m).FirstOrDefault();
                if (item == Money.Zero)
                    continue;
                
                _list.Remove(item);

                list.Add(item);
            }

            Calculate();

            return list.ToArray();
        }

        /// <summary>
        /// Получить наличные с использованием другого счета
        /// </summary>
        public Money[] GetAll(Account source)
        {
            if (source == null)
                source = new Account();
            
            // Попытка возможности выполнения
            GetAll(Clone(), source.Clone());

            // Если проверка прошла успешна, то выполнить операцию 
            return GetAll(this, source);
        }

        private Money[] GetAll(Account target, Account source)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (source == null)
                throw new ArgumentNullException("source");

            source.Reset();

            var amount = target.TotalSum;
            if (amount == Money.Zero)
            {
                source.Add(this);
                target.Clear();

                return new Money[0];
            }

            var result = new List<Money>();
            var sum = Money.Zero;

            var list = source.Add(target).ToSortedList();

            for (var i = 0; i < list.Count; i++)
            {
                var money = list[i];

                sum += money;

                if (sum <= amount)
                {
                    result.Add(money);
                    source.Remove(money);

                    if (sum == amount)
                    {
                        target.Clear();
                        break;
                    }
                }
                else
                {
                    sum -= money;
                    continue;
                }
            }
            if (sum != amount)
                throw VMException.NoChange;

            return result.ToArray();
        }

        private void Calculate()
        {
            var sum = Money.Zero;
            foreach (var m in _list)
            {
                sum += m;
            }
            Sum = sum;

            Notify(() => Sum);
            Notify(() => TotalSum);
        }

        private void Reset()
        {
            _plus = Money.Zero;
            _minus = Money.Zero;
        }

        private IList<Money> ToSortedList()
        {
            var list = this.ToList();
            list.Sort();
            list.Reverse();

            return list;
        }

        public override String ToString()
        {
            return String.Format("Счет на {0}", TotalSum);
        }

        #endregion

        #region IEnumerable<Money> Members

        public IEnumerator<Money> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region ICloneable Members

        public Account Clone()
        {
            var other = new Account();
            other._plus = this._plus;
            other._minus = this._minus;
            other._list.AddRange(this);
            other.Calculate();
            return other;
        }

        Object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion
    }
}
