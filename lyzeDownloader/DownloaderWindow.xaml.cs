using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Linq.Expressions;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System.Net.Http;

namespace lyzeDownloader
{
    /// <summary>
    /// Interaktionslogik für DownloaderWindow.xaml
    /// </summary>
    public partial class DownloaderWindow : Window
    {
        public DownloaderWindow()
        {
            InitializeComponent();

            #region windowButtons - Min,Close,
            minbutton.Click += (s, e) => WindowState = WindowState.Minimized;
            closebtn.Click += (s, e) => Close();
            #endregion
        }

        #region WindowLoaded

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #region Auto Download

            versionNumber.Text = "V" + Application.ResourceAssembly.ManifestModule.Assembly.GetName().Version.ToString();

            HttpClient httpClient = new HttpClient();

            var result = await httpClient.GetAsync(PathStrings.VersionURL);
            var strServerVersion = await result.Content.ReadAsStringAsync();
            var serverVersion = Version.Parse(strServerVersion);
            var thisVersion = Application.ResourceAssembly.ManifestModule.Assembly.GetName().Version;
            if (serverVersion > thisVersion)
            {
                AutoUpdateWindow updateWindow = new AutoUpdateWindow();
                updateWindow.Show();
                this.Close();
            }
            else
            {
                windowMessageBox.IsOpen = true;
                windowMessageBoxIcon.Kind = PackIconKind.Information;
                windowMessageBoxBtn.Content = "Okay";
                windowMessageBoxDownloadBtn.IsEnabled = false;
                windowMessageBoxDownloadBtn.Visibility = Visibility.Hidden;
                windowMessageBoxText.Text = "Du Bist Auf den Neusten Stand.";
            }

            #endregion

            #region iffileexist
            if (File.Exists((PathStrings.desktopPath + PathStrings.MusikPath + "GuteMusik.zip")))
            {
                musikTitle.Text = "Gute Musik - Existiert";
                downloadMusikBtn.IsEnabled = false;
            }
            else
            {
                musikTitle.Text = "Gute Musik";
                downloadMusikBtn.IsEnabled = true;
            }
            if (File.Exists((PathStrings.desktopPath + PathStrings.AdobePath + "Adobe2020.rar")))
            {
                adobeTitle.Text = "Adobe - Existiert";
                downloadAdobeBtn.IsEnabled = false;
            }
            else
            {
                adobeTitle.Text = "Adobe";
                downloadAdobeBtn.IsEnabled = true;
            }
            if (File.Exists((PathStrings.desktopPath + PathStrings.FLStudio20Path + "FLStudio20.rar")))
            {
                FLS20Title.Text = "FLStudio 20 - Existiert";
                downloadFLS20Btn.IsEnabled = false;
            }
            else
            {
                FLS20Title.Text = "FLStudio 20";
                downloadFLS20Btn.IsEnabled = true;
            }
            #endregion
        }

        #endregion

        #region DownloadButtons

        //MysteryMusicBTN
        private void downloadMusikBtn_Click(object sender, RoutedEventArgs e)
        {
            closebtn.IsEnabled = false;

            downloadAdobeBtn.IsEnabled = false;
            downloadMusikBtn.IsEnabled = false;
            downloadFLS20Btn.IsEnabled = false;

            ProgressBarMusik.IsEnabled = true;
            ProgressbrAdobe.IsEnabled = false;
            ProgressbrFLS20.IsEnabled = false;

            ProgressbrFLS20.Visibility = Visibility.Collapsed;
            ProgressbrAdobe.Visibility = Visibility.Collapsed;
            ProgressBarMusik.Visibility = Visibility.Visible;

            WebClient client = new WebClient();
            Directory.CreateDirectory(PathStrings.desktopPath + PathStrings.MusikPath);
            client.DownloadProgressChanged += client_DownloadProgressChanged;
            client.DownloadFileCompleted += client_DownloadFileCompleted;
            client.DownloadFileAsync(new Uri(PathStrings.MusikUrl), PathStrings.desktopPath + PathStrings.MusikPath + "GuteMusik.zip");
        }

        //AdobeBTN
        private void downloadAdobeBtn_Click(object sender, RoutedEventArgs e)
        {
            closebtn.IsEnabled = false;

            downloadAdobeBtn.IsEnabled = false;
            downloadMusikBtn.IsEnabled = false;
            downloadFLS20Btn.IsEnabled = false;

            ProgressBarMusik.IsEnabled = false;
            ProgressbrAdobe.IsEnabled = true;
            ProgressbrFLS20.IsEnabled = false;

            ProgressbrFLS20.Visibility = Visibility.Collapsed;
            ProgressbrAdobe.Visibility = Visibility.Visible;
            ProgressBarMusik.Visibility = Visibility.Collapsed;

            WebClient client = new WebClient();
            Directory.CreateDirectory(PathStrings.desktopPath + PathStrings.AdobePath);
            client.DownloadProgressChanged += client_DownloadProgressChanged;
            client.DownloadFileCompleted += client_DownloadFileCompleted;
            client.DownloadFileAsync(new Uri(PathStrings.AdobeUrl), PathStrings.desktopPath + PathStrings.AdobePath + "Adobe2020.rar");
        }

        //FLStudio 20 BTN
        private void downloadFLS20Btn_Click(object sender, RoutedEventArgs e)
        {
            closebtn.IsEnabled = false;

            downloadAdobeBtn.IsEnabled = false;
            downloadMusikBtn.IsEnabled = false;
            downloadFLS20Btn.IsEnabled = false;

            ProgressBarMusik.IsEnabled = false;
            ProgressbrAdobe.IsEnabled = false;
            ProgressbrFLS20.IsEnabled = true;

            ProgressbrFLS20.Visibility = Visibility.Visible;
            ProgressbrAdobe.Visibility = Visibility.Collapsed;
            ProgressBarMusik.Visibility = Visibility.Collapsed;

            WebClient client = new WebClient();
            Directory.CreateDirectory(PathStrings.desktopPath + PathStrings.FLStudio20Path);
            client.DownloadProgressChanged += client_DownloadProgressChanged;
            client.DownloadFileCompleted += client_DownloadFileCompleted;

            
            client.DownloadFileAsync(new Uri(PathStrings.FLStudio20Url), PathStrings.desktopPath + PathStrings.FLStudio20Path + "FLStudio20.rar");
        }

        #endregion

        #region WhileDownloadIsActive
        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            closebtn.IsEnabled = false;
            infoText.Text = $"Lade {e.BytesReceived} Bytes Runter ({e.BytesReceived / 1046576} MB)";
        }
        #endregion

        #region onDownloadComplete
        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if (WindowState.Equals(WindowState.Minimized))
                {
                    WindowState = WindowState.Normal;

                    closebtn.IsEnabled = true;
                    windowMessageBox.IsOpen = true;
                    windowMessageBoxIcon.Kind = PackIconKind.Information;
                    windowMessageBoxBtn.Content = "Okay";
                    windowMessageBoxDownloadBtn.IsEnabled = false;
                    windowMessageBoxBtn.IsEnabled = true;
                    windowMessageBoxDownloadBtn.Visibility = Visibility.Hidden;
                    windowMessageBoxText.Text = "Fertig Heruntergeladen";
                }
                else
                {
                    closebtn.IsEnabled = true;
                    windowMessageBox.IsOpen = true;
                    windowMessageBoxIcon.Kind = PackIconKind.Information;
                    windowMessageBoxBtn.Content = "Okay";
                    windowMessageBoxBtn.IsEnabled = true;
                    windowMessageBoxDownloadBtn.IsEnabled = false;
                    windowMessageBoxDownloadBtn.Visibility = Visibility.Hidden;
                    windowMessageBoxText.Text = "Fertig Heruntergeladen";
                }
            }
            catch (Exception ex)
            {
                windowMessageBox.IsOpen = true;
                windowMessageBoxIcon.Kind = PackIconKind.Error;
                windowMessageBoxBtn.Content = "Okay";
                windowMessageBoxDownloadBtn.IsEnabled = false;
                windowMessageBoxBtn.IsEnabled = true;
                windowMessageBoxDownloadBtn.Visibility = Visibility.Hidden;
                windowMessageBoxText.Text = ex.Message;
            }

            downloadAdobeBtn.IsEnabled = true;
            downloadMusikBtn.IsEnabled = true;
            downloadFLS20Btn.IsEnabled = true;

            ProgressBarMusik.IsEnabled = false;
            ProgressbrAdobe.IsEnabled = false;
            ProgressbrFLS20.IsEnabled = false;

            ProgressbrFLS20.Visibility = Visibility.Collapsed;
            ProgressbrAdobe.Visibility = Visibility.Collapsed;
            ProgressBarMusik.Visibility = Visibility.Collapsed;


            #region iffileexist
            if (File.Exists((PathStrings.desktopPath + PathStrings.MusikPath + "GuteMusik.zip")))
            {
                musikTitle.Text = "Gute Musik - Existiert";
                downloadMusikBtn.IsEnabled = false;
            }
            else
            {
                musikTitle.Text = "Gute Musik";
                downloadMusikBtn.IsEnabled = true;
            }
            if (File.Exists((PathStrings.desktopPath + PathStrings.AdobePath + "Adobe2020.rar")))
            {
                adobeTitle.Text = "Adobe - Existiert";
                downloadAdobeBtn.IsEnabled = false;
            }
            else
            {
                adobeTitle.Text = "Adobe";
                downloadAdobeBtn.IsEnabled = true;
            }
            if (File.Exists((PathStrings.desktopPath + PathStrings.FLStudio20Path + "FLStudio20.rar")))
            {
                FLS20Title.Text = "FLStudio 20 - Existiert";
                downloadFLS20Btn.IsEnabled = false;
            }
            else
            {
                FLS20Title.Text = "FLStudio 20";
                downloadFLS20Btn.IsEnabled = true;
            }
            #endregion

            infoText.Text = "Warte auf Befehle";
        }
        #endregion
    }
}
