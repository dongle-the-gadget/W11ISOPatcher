using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CdImage;
using System.IO;

namespace W11ISO.Pages
{
    /// <summary>
    /// Interaction logic for PatchingPage.xaml
    /// </summary>
    public partial class PatchingPage : Page
    {
        public PatchingPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    string drive = $"{PowerShellMount.Mount(MainWindow.location.OrigISO)}:\\";

                    Dispatcher.Invoke(() =>
                    {
                        ProgressText.Content = "Copying ISO content to working directory...";
                    });

                    DirectoryCopy(drive, System.IO.Path.Combine(MainWindow.location.WorkingDir, "isoroot"), true);

                    Dispatcher.Invoke(() =>
                    {
                        ProgressText.Content = "Dismounting ISO...";
                    });

                    PowerShellMount.Dismount(MainWindow.location.OrigISO);

                    Dispatcher.Invoke(() =>
                    {
                        ExtractCircle.Fill = new SolidColorBrush(Colors.Green);
                        MountCircle.Fill = new SolidColorBrush(Colors.Blue);
                    });
                }
                catch
                {
                    Dispatcher.Invoke(() =>
                    {
                        ExtractCircle.Fill = new SolidColorBrush(Colors.Red);
                        ProgressText.Content = "Sorry, but the program encountered an error during ISO extraction and cannot continue.";
                        ProgressBar.IsIndeterminate = false;
                    });
                }
            });
        }

        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = System.IO.Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = System.IO.Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }
    }
}
