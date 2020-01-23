using System.Windows;
using AnotherClient.ViewModels;
using Caliburn.Micro;

namespace AnotherClient
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);

            DisplayRootViewFor<ShellViewModel>();
        }
    }
}
