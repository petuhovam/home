using System;
using System.Windows.Input;

namespace VendingMachine.UI.WPF.Commands
{
    public class Command<T> : ICommand
    {
        #region Members

        protected Action<T> execute;

        protected Predicate<T> canExecute;

        private event EventHandler CanExecuteChangedInternal;

        #endregion

        #region ctor

        protected Command()
        { 
        }
        public Command(Action<T> execute)
            : this(execute, p => true)
        {
        }
        public Command(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            
            if (canExecute == null)
                throw new ArgumentNullException("canExecute");
            
            this.execute = execute;
            this.canExecute = canExecute;
        }

        #endregion

        #region ICommand Members

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                this.CanExecuteChangedInternal += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
                this.CanExecuteChangedInternal -= value;
            }
        }

        public Boolean CanExecute(Object parameter)
        {
            return this.execute != null && this.canExecute != null && this.canExecute((T)parameter);
        }

        public void Execute(Object parameter)
        {
            if (this.execute != null)
                this.execute((T)parameter);
        }

        #endregion

        #region Methods

        public void OnCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChangedInternal;
            if (handler != null)
            {
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}
