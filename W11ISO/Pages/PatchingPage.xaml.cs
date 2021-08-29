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
using Microsoft.Dism;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.ComponentModel;
using System.Diagnostics;

namespace W11ISO.Pages
{
    /// <summary>
    /// Interaction logic for PatchingPage.xaml
    /// </summary>
    public partial class PatchingPage : Page
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern Int32 RegLoadKey(UInt32 hKey, String lpSubKey, String lpFile);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern Int32 RegUnLoadKey(UInt32 hKey, String lpSubKey);

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
                        ProgressText.Content = "Mounting boot.wim...";
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
                    return;
                }

                try
                {
                    RemoveReadOnly(System.IO.Path.Combine(MainWindow.location.WorkingDir, "isoroot"));
                    Directory.CreateDirectory(System.IO.Path.Combine(MainWindow.location.WorkingDir, "mount"));
                    DismApi.Initialize(DismLogLevel.LogErrorsWarnings);
                    DismApi.MountImage(System.IO.Path.Combine(MainWindow.location.WorkingDir, "isoroot", "sources", "boot.wim"), System.IO.Path.Combine(MainWindow.location.WorkingDir, "mount"), 2);
                    Dispatcher.Invoke(() =>
                    {
                        MountCircle.Fill = new SolidColorBrush(Colors.Green);
                        RegistryCircle.Fill = new SolidColorBrush(Colors.Blue);
                        ProgressText.Content = "Applying registry patch...";
                    });
                }
                catch
                {
                    Dispatcher.Invoke(() =>
                    {
                        MountCircle.Fill = new SolidColorBrush(Colors.Red);
                        ProgressText.Content = "Sorry, but the program encountered an error during boot.wim mounting and cannot continue.";
                        ProgressBar.IsIndeterminate = false;
                    });
                    return;
                }

                try
                {
                    Privileges.EnablePrivilege(SecurityEntity.SE_BACKUP_NAME);
                    Privileges.EnablePrivilege(SecurityEntity.SE_RESTORE_NAME);

                    Process process = new();
                    process.StartInfo.FileName = "reg.exe";
                    process.StartInfo.Arguments = $"load HKLM\\bootwimpatch \"{System.IO.Path.Combine(MainWindow.location.WorkingDir, "mount", "Windows", "system32", "config", "SYSTEM")}\"";
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                    process.WaitForExit();

                    Registry.SetValue("HKEY_LOCAL_MACHINE\\bootwimpatch\\Setup\\LabConfig", "BypassTPMCheck", 1);
                    Registry.SetValue("HKEY_LOCAL_MACHINE\\bootwimpatch\\Setup\\LabConfig", "BypassSecureBootCheck", 1);
                    Registry.SetValue("HKEY_LOCAL_MACHINE\\bootwimpatch\\Setup\\LabConfig", "BypassRAMCheck", 1);

                    process = new();
                    process.StartInfo.FileName = "reg.exe";
                    process.StartInfo.Arguments = $"unload HKLM\\bootwimpatch";
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                    process.WaitForExit();
                    Dispatcher.Invoke(() =>
                    {
                        RegistryCircle.Fill = new SolidColorBrush(Colors.Green);
                        UnmountCircle.Fill = new SolidColorBrush(Colors.Blue);
                        ProgressText.Content = "Saving and unmounting boot.wim...";
                    });
                }
                catch
                {
                    Dispatcher.Invoke(() =>
                    {
                        RegistryCircle.Fill = new SolidColorBrush(Colors.Red);
                        ProgressText.Content = "Sorry, but the program encountered an error during registry patching and cannot continue.";
                        ProgressBar.IsIndeterminate = false;
                    });
                    return;
                }

                try
                {
                    DismApi.UnmountImage(System.IO.Path.Combine(MainWindow.location.WorkingDir, "mount"), true);
                    Dispatcher.Invoke(() =>
                    {
                        UnmountCircle.Fill = new SolidColorBrush(Colors.Green);
                        PackageCircle.Fill = new SolidColorBrush(Colors.Blue);
                        ProgressText.Content = "Building ISO file...";
                    });
                }
                catch
                {
                    Dispatcher.Invoke(() =>
                    {
                        UnmountCircle.Fill = new SolidColorBrush(Colors.Red);
                        ProgressText.Content = "Sorry, but the program encountered an error during boot.wim saving and cannot continue.";
                        ProgressBar.IsIndeterminate = false;
                    });
                    return;
                }

                try
                {
                    CdImageHandler.GenerateISOImage(new FileInfo(MainWindow.location.Product).FullName, System.IO.Path.Combine(MainWindow.location.WorkingDir, "isoroot"), "CCCOMA_X64FRE_EN-US_DV9", (Operation, ProgressPercentage, IsIndeterminate) =>
                    {
                        Dispatcher.Invoke(() =>
                        {
                            ProgressBar.IsIndeterminate = IsIndeterminate;
                            if (!IsIndeterminate)
                            {
                                ProgressBar.Value = ProgressPercentage;
                            }
                        });
                    });

                    Dispatcher.Invoke(() =>
                    {
                        PackageCircle.Fill = new SolidColorBrush(Colors.Green);
                        CleaningUpCircle.Fill = new SolidColorBrush(Colors.Blue);
                        ProgressText.Content = "Deleting unused files...";
                    });
                }
                catch
                {
                    Dispatcher.Invoke(() =>
                    {
                        PackageCircle.Fill = new SolidColorBrush(Colors.Red);
                        ProgressText.Content = "Sorry, but the program encountered an error during new ISO creation and cannot continue.";
                        ProgressBar.IsIndeterminate = false;
                    });
                }

                try
                {
                    DirectoryInfo dir = new DirectoryInfo(MainWindow.location.WorkingDir);
                    foreach (var file in dir.GetFiles())
                        file.Delete();
                    foreach (var folder in dir.GetDirectories())
                        folder.Delete(true);
                    Dispatcher.Invoke(() =>
                    {
                        CleaningUpCircle.Fill = new SolidColorBrush(Colors.Green);
                        ProgressText.Content = "Done!";
                    });
                }
                catch
                {
                    Dispatcher.Invoke(() =>
                    {
                        CleaningUpCircle.Fill = new SolidColorBrush(Colors.Red);
                        ProgressText.Content = "Sorry, we failed to clean up, but your ISO file is ready. You can manually clean up by deleting the contents of the working folder, specified in the previous step.";
                    });
                }
                finally
                {
                    Dispatcher.Invoke(() =>
                    {
                        ProgressBar.IsIndeterminate = false;
                    });
                }
            });
        }

        private void RemoveReadOnly(string workingDir)
        {
            foreach (var file in new DirectoryInfo(workingDir).GetFiles("*", SearchOption.AllDirectories))
                file.Attributes &= ~FileAttributes.ReadOnly;
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
