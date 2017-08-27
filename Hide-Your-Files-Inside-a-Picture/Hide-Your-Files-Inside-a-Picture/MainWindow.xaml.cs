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
        page frameState;
        pgAddFiles addfile;
        pgLogIn logOn;
        pgAddText addText;
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
                case page.logOn:  frame.Navigate(logOn); frameState = page.logOn;
                    break;
                case page.AddFiles:  frame.Navigate(addfile); frameState = page.AddFiles;
                    break;
                case page.AddText:  frame.Navigate(addText); frameState = page.AddText;
                    break;
                default:    break;
            }
        }
        
        private void loadUsers()
        {
            Directory.CreateDirectory(savePath);
            try
            {
                if (System.IO.File.Exists(saveFile))
                {
                    //UserList.Clear();
                    //UserList = JsonConvert.DeserializeObject<List<User>>(System.IO.File.ReadAllText(saveFile));
                }
                else
                {
                    Gtools.createFile(saveFile, "[ ]");
                    loadUsers();
                }
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message + " Load User Error.\n");

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
            addText = new pgAddText(this);
            changePage(page.logOn);
            //frame.Source = new Uri("/Hide-Your-Files-Inside-a-Picture;component/Pages/pgAddFiles.xaml", UriKind.Relative);
        }

        private void mnuExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void mnuAddText_Click(object sender, RoutedEventArgs e)
        {
            changePage(page.AddText);
        }

        private void mnuHideFiles(object sender, RoutedEventArgs e)
        {
            changePage(page.AddFiles);
        }

        private void mnuDownload_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Obrelix/Hide-Your-Files-Inside-a-Picture");
        }

        private void mnuLogIn_Click(object sender, RoutedEventArgs e)
        {
            changePage(page.logOn);
        }

        private void mnuReport_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Obrelix/Hide-Your-Files-Inside-a-Picture/issues");
        }

        private void frame_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
           
        }

        private void frame_Navigated(object sender, NavigationEventArgs e)
        {
            switch (frameState)
            {
                case page.logOn:
                    mnuMainMenu.IsEnabled = false;
                    break;
                case page.AddFiles:
                    mnuMainMenu.IsEnabled = true;
                    break;
                case page.AddText:
                    mnuMainMenu.IsEnabled = true;
                    break;
                default:
                    break;
            }
        }

        #endregion


    }
}
