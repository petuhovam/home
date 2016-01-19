using System;
using System.Collections;
using System.Collections.Generic;

using VendingMachine.Domain.Models;

namespace VendingMachine.UI.AspNetMvc.Models
{
    public class AccountModel : IEnumerable<MoneyCount>
    {
        #region Members

        readonly IDictionary<Money, MoneyCount> _items = new SortedDictionary<Money, MoneyCount>();
        readonly Account _account;

        #endregion

        #region ctor

        public AccountModel(Account account)
        {
            if (account == null)
                throw new ArgumentNullException("account");

            _account = account;
        }

        #endregion

        #region Properties

        public Money TotalSum
        {
            get { return _account.TotalSum; }
        }

        #endregion

        #region Methods

        public void Refresh()
        {
            _items.Clear();

            foreach (var m in _account)
            {
                if (_items.ContainsKey(m))
                    _items[m].Count += 1;
                else
                    _items[m] = new MoneyCount() { Money = m, Count = 1 };
            }
        }

        #endregion

        #region IEnumerable<MoneyCount> Members

        public IEnumerator<MoneyCount> GetEnumerator()
        {
            return _items.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}