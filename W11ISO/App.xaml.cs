using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace W11ISO
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            if (Environment.OSVersion.Version.Build < 19041)
            {
                MessageBoxResult msgBox = MessageBox.Show(
                    "You are using W11ISOPatcher on a non-supported Windows version. " +
                    "We are not responsible for any damages and will not offer official support for these versions." +
                    Environment.NewLine +
                    "Are you sure you want to continue?",
                    "Unsupported Windows version",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);
                if (msgBox == MessageBoxResult.No)
                {
                    Shutdown();
                    return;
                }
            }

            base.OnStartup(e);
        }
    }
}
