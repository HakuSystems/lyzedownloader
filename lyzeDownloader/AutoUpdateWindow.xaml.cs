using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace lyzeDownloader
{
    /// <summary>
    /// Interaction logic for AutoUpdateWindow.xaml
    /// </summary>
    public partial class AutoUpdateWindow : Window
    {
        public AutoUpdateWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #region AutoDownload
            HttpClient httpClient = new HttpClient();

            var result = await httpClient.GetAsync(PathStrings.VersionURL);
            var strServerVersion = await result.Content.ReadAsStringAsync();
            var serverVersion = Version.Parse(strServerVersion);
            var thisVersion = Application.ResourceAssembly.ManifestModule.Assembly.GetName().Version;
            if (serverVersion > thisVersion)
            {
                windowMessageBox.IsOpen = true;
                windowMessageBoxIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Information;
                windowMessageBoxBtn.IsEnabled = false;
                windowMessageBoxBtn.Visibility = Visibility.Visible;
                windowMessageBoxText.Text = "Downloade Neueste Version..";


                WebClient client = new WebClient();
                Directory.CreateDirectory(PathStrings.desktopPath + PathStrings.AppExecutablePath);
                client.DownloadProgressChanged += client_DownloadProgressChanged;
                client.DownloadFileCompleted += client_DownloadFileCompleted;
                client.DownloadFileAsync(new Uri(PathStrings.AppExecutableURL), PathStrings.desktopPath + PathStrings.AppExecutablePath + $"lyzeDownloaderUpdated.exe");
            }
            else
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            #endregion
        }

        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            windowMessageBoxText2.Text = $"Lade {e.BytesReceived} Bytes Runter ({e.BytesReceived / 1046576} MB)";
        }
    }
}
