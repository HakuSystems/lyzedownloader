using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
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

namespace lyzeDownloader
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Directory.CreateDirectory(PathStrings.desktopPath + "\\lyzeDownloader");
                DownloaderWindow downloader = new DownloaderWindow();
                downloader.Show();
                this.Close();

            }
            catch (Exception ex)
            {
                windowMessageBox.IsOpen = true;
                windowMessageBoxBtn.Content = "Okay";
                windowMessageBoxText.Text = ex.Message;
            }

        }
    }
}
