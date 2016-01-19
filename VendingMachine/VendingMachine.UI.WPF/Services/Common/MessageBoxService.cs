using System;
using System.Windows;
using System.ComponentModel.Composition;

using VendingMachine.Domain.Services.Common;

namespace VendingMachine.UI.WPF.Services.Common
{
    [PartCreationPolicy(CreationPolicy.Shared)]
    [Export(typeof(IMsgService))]
    class MessageBoxService : IMsgService
    {
        const String WindowTitle = "Торговый автомат";

        #region Methods

        private void ShowMessage(String message, MessageBoxButton button, MessageBoxImage image)
        {
            MessageBox.Show(message, WindowTitle, button, image);
        }

        #endregion

        #region IMsgService Members

        public void Show(String format, params Object[] args)
        {
            ShowMessage(String.Format(format, args), MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void Error(String format, params Object[] args)
        {
            ShowMessage(String.Format(format, args), MessageBoxButton.OK, MessageBoxImage.Error);
        }

        #endregion
    }
}
