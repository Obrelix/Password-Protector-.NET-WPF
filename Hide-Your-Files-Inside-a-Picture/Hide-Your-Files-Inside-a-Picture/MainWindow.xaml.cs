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

namespace Hide_Your_Files_Inside_a_Picture
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = "*.jpg";
            openFileDialog.Filter = "Image Files|*.jpg";
            if (openFileDialog.ShowDialog() == true)
                image.Source = new BitmapImage(new Uri(openFileDialog.FileName));
        }

        private void btnFile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {

        }

        private void image_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = (e.Data.GetDataPresent(DataFormats.FileDrop)) ? DragDropEffects.All : DragDropEffects.None;
        }

        private void image_Drop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (string file in files)
            {
                if (System.IO.Path.GetExtension(file) == ".jpg" || System.IO.Path.GetExtension(file) == ".JPG")
                {
                    image.Source = new BitmapImage(new Uri(file));
                }
                else
                {
                    MessageBox.Show("File is not valid image."+ Environment.NewLine+"Please insert a jpg image.", 
                        "Image format error!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
        }
    }
}
