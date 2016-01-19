using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using VendingMachine.Domain.Models;

using VendingMachine.UI.WPF.Models;

namespace VendingMachine.UI.WPF.Models
{
    public class AccountModel : NotifyItem, IEnumerable<MoneyCount>, INotifyCollectionChanged
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

        #region Methods

        public void Refresh()
        {
            _items.Clear();

            foreach (var m in _account)
            {
                if (_items.ContainsKey(m))
                    _items[m].Count += 1;
                else
                    _items[m] = new MoneyCount() { Money = m, Count= 1 };
            }

            ResetCollection();
        }

        private void ResetCollection()
        {
            var handler = CollectionChanged;
            if (handler != null)
            {
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                handler(this, e);
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

        #region INotifyCollectionChanged Members

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion
    }
}
