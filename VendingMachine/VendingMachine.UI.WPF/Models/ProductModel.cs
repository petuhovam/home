using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using VendingMachine.Domain.Models;

using VendingMachine.UI.WPF.Models;

namespace VendingMachine.UI.WPF.Models
{
    public class ProductModel : NotifyItem, IEnumerable<ProductCount>, INotifyCollectionChanged
    {
        #region Members

        readonly IDictionary<String, ProductCount> _items = new Dictionary<String, ProductCount>();
        readonly IList<Product> _products;

        #endregion

        #region ctor

        public ProductModel(IList<Product> products)
        {
            if (products == null)
                throw new ArgumentNullException("products");

            _products = products;
        }

        #endregion

        #region Properties

        public Money TotalPrice
        {
            get;
            private set;
        }

        #endregion

        #region Methods

        public void Refresh()
        {
            TotalPrice = Money.Zero;

            _items.Clear();
            
            foreach (var p in _products)
            {
                if (_items.ContainsKey(p.Name))
                    _items[p.Name].Count += 1;
                else
                    _items[p.Name] = new ProductCount() { Product = p, Count= 1 };

                TotalPrice += p.Price;
            }

            ResetCollection();
            Notify(() => TotalPrice);
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

        #region IEnumerable<ProductCount> Members

        public IEnumerator<ProductCount> GetEnumerator()
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
