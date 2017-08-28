using Microsoft.Win32;
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
    /// Interaction logic for pgAddFiles.xaml
    /// </summary>
    public partial class pgAddFiles : Page
    {
        public pgAddFiles(MainWindow parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
        }

        #region General Declaretion

        private List<FileIO> fileList = new List<FileIO>();
        private string imagePath = string.Empty;
        
        private MainWindow parentWindow;

        #endregion

        #region Functions / Procedures


        private void updateListBox()
        {
            try
            {
                lsbFiles.ItemsSource = null;
                lsbFiles.ItemsSource = fileList;
            }
            catch (Exception) { throw; }
        }

        private void hideFiles()
        {
            try
            {

                string tempDirectory, imageName, tempImagePath,tempZipPath;
                List<string> tempFileList = new List<string>();
                List<string> commandsList = new List<string>();

                tempDirectory = System.IO.Path.Combine(MainWindow.savePath, string.Format("{0:dd-MM-yyyy HH_mm_ss}", DateTime.Now));
                Directory.CreateDirectory(@tempDirectory);
                imageName = System.IO.Path.GetFileName(imagePath);
                tempImagePath = System.IO.Path.Combine(tempDirectory, imageName);
                File.Copy(imagePath, tempImagePath);

                tempZipPath = Gtools.compressFile(tempDirectory, fileList);
                for (int i = 0; i <= fileList.Count - 1; i++)
                {
                    fileList[i].zipPath = tempZipPath;
                }

                commandsList.Add("cd \"" + @tempDirectory + "\"");
                commandsList.Add(@"Copy /b " + "\"" + @imageName + "\"" + @" + " + "\"" +
                    System.IO.Path.GetFileName(@tempZipPath) + "\"" +
                    @" " + "\"" + @imageName + "\"");
                Gtools.ExecuteCMDCommands(commandsList);


                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Save your new image.";
                saveFileDialog.DefaultExt = "*.jpg";
                saveFileDialog.Filter = "Image Files|*.jpg";
                if (saveFileDialog.ShowDialog() == true)
                {
                    string imagePath = saveFileDialog.FileName;
                    if (!System.IO.Path.HasExtension(imagePath)) imagePath += ".jpg";
                    File.Copy(tempImagePath, imagePath);
                    MessageBox.Show("For accessing your hidden files open the new image " + Environment.NewLine +
                      "with a file compression program like winrar, 7zip etc.", "New image saved !", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                deleteDummies(tempDirectory);
            }
            catch (Exception) { throw; }

        }

        private void deleteDummies(string path)
        {
            DirectoryInfo di = new DirectoryInfo(@path);

            foreach (FileInfo file in di.GetFiles()) file.Delete();

            foreach (DirectoryInfo dir in di.GetDirectories()) dir.Delete(true);

            di.Delete(true);
        }

        #endregion

        #region Event Handlers

        private void btnImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.DefaultExt = "*.jpg";
                openFileDialog.Filter = "Image Files|*.jpg";
                if (openFileDialog.ShowDialog() == true)
                {
                    imagePath = openFileDialog.FileName;
                    image.Source = new BitmapImage(new Uri(imagePath));
                }
            }
            catch (Exception) { throw; }

        }

        private void btnFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    fileList.Add(new FileIO(openFileDialog.FileName));
                }
                updateListBox();
            }
            catch (Exception) { throw; }
        }

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            if (fileList.Count < 1 || string.IsNullOrEmpty(imagePath))
            {
                string outPutString = (fileList.Count < 1) ?
                    "Please add any file first." :
                    "Please add any jpg Image first.";
                MessageBox.Show(outPutString,
                        "Compress Error!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            else hideFiles();

        }

        private void image_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = (e.Data.GetDataPresent(DataFormats.FileDrop)) ? DragDropEffects.All : DragDropEffects.None;
        }

        private void image_Drop(object sender, DragEventArgs e)
        {
            try
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string file in files)
                {
                    if (System.IO.Path.GetExtension(file) == ".jpg" || System.IO.Path.GetExtension(file) == ".JPG")
                    {
                        image.Source = new BitmapImage(new Uri(file));
                        imagePath = file;
                    }
                    else
                    {
                        MessageBox.Show("File is not valid image." + Environment.NewLine + "Please insert a jpg image.",
                            "Image format error!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                }
            }
            catch (Exception) { throw; }

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                fileList.Clear();
                updateListBox();
            }
            catch (Exception) { throw; }
        }

        private void lsbFiles_Drop(object sender, DragEventArgs e)
        {
            try
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string file in files)
                {
                    fileList.Add(new FileIO(file));
                    updateListBox();
                }
            }
            catch (Exception) { throw; }

        }

        private void lsbFiles_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = (e.Data.GetDataPresent(DataFormats.FileDrop)) ? DragDropEffects.All : DragDropEffects.None;
        }

        private void expander_Expanded(object sender, RoutedEventArgs e)
        {
            this.Height += 100;
            parentWindow.changeHeight(this.Height + 50);
        }

        private void lsbFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void expander_Collapsed_1(object sender, RoutedEventArgs e)
        {
            this.Height -= 100;
            parentWindow.changeHeight(this.Height + 50);

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            parentWindow.changeHeight(this.Height + 50);
            parentWindow.changeWidth( this.Width + 50);
        }

        private void btnResetImage_Click(object sender, RoutedEventArgs e)
        {
           image.Source = image.Source = new BitmapImage(new Uri(@"pack://application:,,/Resources/Drop Image Here.png")); 
        }

        #endregion

    }
}
