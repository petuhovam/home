using System;
using System.Windows;
using System.Reflection;
using System.ComponentModel.Composition;

using VendingMachine.Domain.Services.Common;

using VendingMachine.UI.WPF.Views;
using VendingMachine.UI.WPF.ViewModels;

namespace VendingMachine.UI.WPF
{
    [Export]
    public class Bootstrapper
    {
        #region Properties

        [Import]
        public MainViewModel ViewModel
        {
            get;
            private set;
        }

        #endregion

        #region Methods

        public Window BuildView()
        {
            var view = new MainWindow() { DataContext = ViewModel };
            view.Loaded += (s, e) => ViewModel.Build();
            return view;
        }

        #endregion
    }
}
