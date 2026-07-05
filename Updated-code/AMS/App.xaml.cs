using System.Windows;
using AMS.Services;
using AMS.Views;

namespace AMS
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Prevent app from shutting down when the login window closes
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            // Show login window first
            var login = new LoginWindow();
            bool? result = login.ShowDialog();
            if (result != true)
            {
                Shutdown();
                return;
            }

            // Launch main window
            var main = new MainWindow();
            this.MainWindow = main;
            this.ShutdownMode = ShutdownMode.OnMainWindowClose;
            main.Show();
        }
    }
}
