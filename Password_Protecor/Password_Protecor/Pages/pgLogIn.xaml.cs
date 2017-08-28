using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
        private string username, password;
        private List<User> userList = new List<User>();

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

        private void loadUsers()
        {
            Directory.CreateDirectory(MainWindow.savePath);
            try
            {
                if (System.IO.File.Exists(MainWindow.saveFile))
                {
                    userList.Clear();
                    userList = JsonConvert.DeserializeObject<List<User>>(System.IO.File.ReadAllText(MainWindow.saveFile));
                }
                else
                {
                    Gtools.createFile(MainWindow.saveFile, "[ ]");
                    loadUsers();
                }
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message + " Load User Error.\n");

            }
        }

        private void saveUsers()
        {
            try
            {
                string contentsToWriteToFile = JsonConvert.SerializeObject(userList.ToArray(), Newtonsoft.Json.Formatting.Indented);

                System.IO.File.WriteAllText(MainWindow.saveFile, contentsToWriteToFile);

            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message + " Save User Error.\n");
            }
        }

        private void addNewUser()
        {
            userList.Add(new User(username, password));
        }

        private bool compareUsers()
        {
            foreach (User user in userList)
            {
                if (user.name == username 
                    && user.password == password) return true;
            }
            return false;
        }

        #endregion

        #region Event Handlers

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            username = string.Empty;
            password = string.Empty;
            loadUsers();
        }

        private void chbNewUser_Checked(object sender, RoutedEventArgs e)
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
            if (txtUserName.Text.Length >= 5 && txtPassword.Password.Length >= 8)
            {
                username = Gtools.hashFromString(Gtools.encodeMix(txtUserName.Text, txtUserName.Text, txtPassword.Password));
                password = Gtools.hashFromString(Gtools.encodeMix(txtPassword.Password, txtUserName.Text, txtPassword.Password));

                //string text = txtUserName.Text;
                //System.Diagnostics.Debug.WriteLine(text);
                //text = Gtools.encodeMix(text, username, password);
                //System.Diagnostics.Debug.WriteLine(text);
                //text = Gtools.decodeMix(text, username, password);
                //System.Diagnostics.Debug.WriteLine(text);

                switch (chbNewUser.IsChecked)
                {
                    case true:
                        MainWindow.username = txtUserName.Text;
                        MainWindow.password = txtPassword.Password;
                        addNewUser();
                        saveUsers();
                        chbNewUser.IsChecked = false;
                        parentWindow.changePage(page.AddText);
                        break;
                    case false:
                        if (compareUsers())
                        {
                            MainWindow.username = txtUserName.Text;
                            MainWindow.password = txtPassword.Password;
                            parentWindow.changePage(page.AddText);
                        }
                        else
                        {
                            MessageBox.Show("User does not exist!",
                                                "Log in Error.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            txtPassword.Clear();
                            txtPassword.Focus();
                        }
                        break;
                    default:
                        break;
                }
            }                
            else MessageBox.Show("The Username must contain at least 5 characters " + Environment.NewLine +
                                    "The Password must contain at least 8 characters", 
                                    "Log in Error.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        

        #endregion

    }
}
