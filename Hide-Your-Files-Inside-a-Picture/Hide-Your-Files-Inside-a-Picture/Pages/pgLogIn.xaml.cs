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

namespace Hide_Your_Files_Inside_a_Picture
{
    /// <summary>
    /// Interaction logic for pgLogIn.xaml
    /// </summary>
    public partial class pgLogIn : Page
    {
        #region Genera Declaretion
        MainWindow parentWindow;
        #endregion

        public pgLogIn(MainWindow window)
        {
            InitializeComponent();
            parentWindow = window;
            KeyGesture CloseCmdKeyGesture = new KeyGesture(Key.L, ModifierKeys.Alt);

            KeyBinding CloseKeyBinding = new KeyBinding(
                ApplicationCommands.Close, CloseCmdKeyGesture);

            this.InputBindings.Add(CloseKeyBinding);
        }

        #region Functions / Procedures

        #endregion

        #region Event Handlers

        #endregion


        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            txtUserName.Focus();
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {      
            if (e.Key == Key.Enter) btnLogON_Click(this, new RoutedEventArgs());
        }

        private void btnLogON_Click(object sender, RoutedEventArgs e)
        {
            parentWindow.changePage(page.AddFiles);
        }
    }
}
