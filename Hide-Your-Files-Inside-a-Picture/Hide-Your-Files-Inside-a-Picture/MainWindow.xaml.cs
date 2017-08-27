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
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Hide_Your_Files_Inside_a_Picture
{
    public enum page : byte
    {
        logOn = 1,
        AddFiles = 2,
        AddText = 3
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        #region General Declaretion

        pgAddFiles addfile;
        pgLogIn logOn;
        public static string savePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Secure_Log";
        public static string saveFile = savePath + "\\dummy.json";

        #endregion

        #region Function / Procedures
        public void changeHeight(double height)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<double>(changeHeight), new object[] { height });
                return;
            }
            this.Height = height;
        }

        public void changeWidth(double width)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<double>(changeWidth), new object[] { width });
                return;
            }
            this.Width = width;
        }

        public void changePage(page page)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<page>(changePage), new object[] { page });
                return;
            }
            switch (page)
            {
                case page.logOn:
                    frame.Navigate(logOn);
                    break;
                case page.AddFiles:
                    frame.Navigate(addfile);
                    break;
                case page.AddText:
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Event Handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(@savePath))
                Directory.CreateDirectory(@savePath);
            logOn = new pgLogIn(this);
            addfile = new pgAddFiles(this);
            frame.Navigate(logOn);
            //frame.Source = new Uri("/Hide-Your-Files-Inside-a-Picture;component/Pages/pgAddFiles.xaml", UriKind.Relative);
        }

        #endregion
    }
}
