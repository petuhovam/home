using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace VendingMachine.Domain.Models
{
    public abstract class NotifyItem : INotifyPropertyChanged
    {
        #region Members

        protected void Notify<T>(Expression<Func<T>> propertyExpression)
        {
            var propertyName = GetMemberName(propertyExpression);

            Notify(propertyName);
        }
        protected void Notify()
        {
            Notify(String.Empty);
        }
        protected void Notify(String propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        protected String GetMemberName<T>(Expression<Func<T>> expression)
        {
            return ((MemberExpression)expression.Body).Member.Name;
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
