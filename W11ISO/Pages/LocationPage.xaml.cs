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
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.Win32;
using System.IO;

namespace W11ISO.Pages
{
    /// <summary>
    /// Interaction logic for LocationPage.xaml
    /// </summary>
    public partial class LocationPage : Page
    {
        public LocationPage()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow._frame.Navigate(typeof(WelcomePage));
        }

        private void OrigISOButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "ISO files|*.iso";
            openFileDialog.FilterIndex = 0;
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != null)
            {
                OrigISOTextBox.Text = openFileDialog.FileName;
            }
        }

        private void WorkingDirButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            dialog.ShowDialog();
            try
            {
                WorkingDirTextBox.Text = dialog.FileName;
            }
            catch { }
        }

        private void ProductButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "ISO files|*.iso";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName != null)
            {
                ProductTextBox.Text = saveFileDialog.FileName;
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            CheckOrig();

            if (OrigISOError.Visibility == Visibility.Visible || WorkingDirError.Visibility == Visibility.Visible || ProductError.Visibility == Visibility.Visible)
            {
                return;
            }
            MainWindow.location = new Location(new FileInfo(OrigISOTextBox.Text).FullName, new DirectoryInfo(WorkingDirTextBox.Text).FullName, new FileInfo(ProductTextBox.Text).FullName);
            MainWindow._frame.Navigate(typeof(PatchingPage));
        }

        private void CheckOrig()
        {
            try
            {
                FileInfo fileInfo = new(OrigISOTextBox.Text);
                if (!fileInfo.Extension.Equals(".iso", StringComparison.InvariantCultureIgnoreCase))
                {
                    OrigISOError.Visibility = Visibility.Visible;
                    OrigISOError.Content = "The specified file is not an ISO file.";
                }
                else if (!fileInfo.Exists)
                {
                    OrigISOError.Visibility = Visibility.Visible;
                    OrigISOError.Content = "The specified ISO file does not exist.";
                }
                else
                {
                    OrigISOError.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                OrigISOError.Visibility = Visibility.Visible;
                OrigISOError.Content = ex.Message;
            }

            try
            {
                DirectoryInfo dirInfo = new(WorkingDirTextBox.Text);
                if (!dirInfo.Exists)
                {
                    WorkingDirError.Visibility = Visibility.Visible;
                    WorkingDirError.Content = "The working directory does not exist.";
                }
                else if (dirInfo.GetDirectories().Length > 0 || dirInfo.GetFiles().Length > 0)
                {
                    WorkingDirError.Visibility = Visibility.Visible;
                    WorkingDirError.Content = "The working directory is not empty.";
                }
                else
                {
                    WorkingDirError.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                WorkingDirError.Visibility = Visibility.Visible;
                WorkingDirError.Content = ex.Message;
            }

            try
            {
                FileInfo fileInfo = new(ProductTextBox.Text);
                if (!fileInfo.Extension.Equals(".iso", StringComparison.InvariantCultureIgnoreCase))
                {
                    ProductError.Visibility = Visibility.Visible;
                    ProductError.Content = "The specified file is not an ISO file.";
                }
                else if (fileInfo.Exists)
                {
                    ProductError.Visibility = Visibility.Visible;
                    ProductError.Content = "The specified ISO file exists.";
                }
                else
                {
                    ProductError.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                ProductError.Visibility = Visibility.Visible;
                ProductError.Content = ex.Message;
            }
        }

        private void OrigISOTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            OrigISOError.Visibility = Visibility.Collapsed;
        }

        private void WorkingDirTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            WorkingDirError.Visibility = Visibility.Collapsed;
        }

        private void ProductTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ProductError.Visibility = Visibility.Collapsed;
        }
    }
}
