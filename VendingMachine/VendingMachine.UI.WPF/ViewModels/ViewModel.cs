using System;
using System.Linq.Expressions;
using System.Windows.Input;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using VendingMachine.Domain.Models;
using VendingMachine.Domain.Services.Common;

using VendingMachine.UI.WPF.Commands;

namespace VendingMachine.UI.WPF.ViewModels
{
    public class ViewModel : NotifyItem
    {
        #region Members

        private readonly IDictionary<String, Object> _props = new Dictionary<String, Object>();

        #endregion

        #region Services

        [Import]
        public IMsgService Msgs
        {
            get;
            private set;
        }

        [Import]
        public ILogsService Logs
        {
            get;
            private set;
        }

        #endregion

        #region Methods

        protected T Get<T>(Expression<Func<T>> propertyExpression)
        {
            return Get<T>(propertyExpression, () => default(T));
        }
        protected T Get<T>(Expression<Func<T>> propertyExpression, Func<T> createObj)
        {
            var propertyName = GetMemberName(propertyExpression);
            return Get(propertyName, createObj);
        }
        protected T Get<T>(String propertyName, Func<T> createObj)
        {
            if (propertyName == null)
                throw new ArgumentNullException("propertyName");

            if (_props.ContainsKey(propertyName) == false)
                Set(propertyName, createObj());

            return (T)_props[propertyName];
        }

        protected void Clear<T>(Expression<Func<T>> propertyExpression)
        {
            var propertyName = GetMemberName(propertyExpression);
            Clear<T>(propertyName);
        }
        protected void Clear<T>(String propertyName)
        {
            if (propertyName == null)
                throw new ArgumentNullException("propertyName");

            if (_props.ContainsKey(propertyName))
            {
                _props.Remove(propertyName);
            }
            Notify(propertyName);
        }

        protected void Set<T>(Expression<Func<T>> propertyExpression, T obj)
        {
            var propertyName = GetMemberName(propertyExpression);
            Set(propertyName, obj);
        }
        protected void Set<T>(String propertyName, T obj)
        {
            if (propertyName == null)
                throw new ArgumentNullException("propertyName");

            _props[propertyName] = obj;

            Notify(propertyName);
        }

        protected Command<T> GetProxyCommand<T>(Command<T> command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            return new Command<T>(p => ExecuteProxyCommand(command, p), p => CanExecuteProxyCommand(command, p));
        }

        protected virtual void ExecuteProxyCommand(ICommand command, Object p)
        {
            try
            {
                command.Execute(p);
            }
            catch (Exception ex)
            {
                Logs.Error(ex);
                Msgs.Error("Ошибка: " + ex.Message);
            }
        }

        protected virtual Boolean CanExecuteProxyCommand(ICommand command, Object p)
        {
            return command.CanExecute(p);
        }

        #endregion
    }
}
