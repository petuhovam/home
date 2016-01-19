using System;
using System.Windows;

using VendingMachine.Domain.Models;
using VendingMachine.Domain.Services.Mef;
using VendingMachine.Domain.Services.Common;

namespace VendingMachine.UI.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceContainer CreateContainer()
        {
            return new MefServiceContainer(GetType().Assembly, typeof(User).Assembly);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mef = CreateContainer();
            var bootstrapper = mef.Resolve<Bootstrapper>();

            MainWindow = bootstrapper.BuildView();
            MainWindow.Show();
        }
    }
}
