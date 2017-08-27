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
    /// Interaction logic for pgAddText.xaml
    /// </summary>
    public partial class pgAddText : Page
    {
        public pgAddText(MainWindow window)
        {
            InitializeComponent();
            parentWindow = window;
        }

        #region General Declaretion

        private string imagePath = string.Empty;
        private string textTohide = string.Empty;

        private MainWindow parentWindow;

        #endregion

        #region Functions / Procedures

        private void hideFiles()
        {
            string tempDirectory, imageName, tempImagePath, tempTextName, tempTextPath, tempTextZipPath;
            tempDirectory = System.IO.Path.Combine(MainWindow.savePath, string.Format("{0:dd-MM-yyyy HH_mm_ss}", DateTime.Now));
            List<string> commandsList = new List<string>();
            try
            {
                Directory.CreateDirectory(@tempDirectory);
                imageName = System.IO.Path.GetFileName(imagePath);
                tempImagePath = System.IO.Path.Combine(tempDirectory, imageName);
                File.Copy(imagePath, tempImagePath);
                tempTextName = "source.txt"; tempTextPath = System.IO.Path.Combine(tempDirectory, tempTextName);
                Gtools.createFile(tempTextPath, textTohide);
                tempTextZipPath = Gtools.compressFile(tempDirectory, tempTextPath);
                commandsList.Add("cd \"" + @tempDirectory + "\"");
                commandsList.Add(@"Copy /b " + "\"" + @imageName + "\"" + @" + " + "\"" +
                    System.IO.Path.GetFileName(tempTextZipPath) + "\"" +
                    @" " + "\"" + @imageName + "\"");
                Gtools.ExecuteCMDCommands(commandsList);


                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Save your image as jpg file.";
                saveFileDialog.DefaultExt = "*.jpg";
                saveFileDialog.Filter = "Image Files|*.jpg";
                if (saveFileDialog.ShowDialog() == true)
                {
                    string imagePath = saveFileDialog.FileName;
                    if (!System.IO.Path.HasExtension(imagePath)) imagePath += ".jpg";
                    File.Copy(tempImagePath, imagePath, true);
                    MessageBox.Show("For accessing your hidden files open the new image " + Environment.NewLine +
                        "with a file compression program like winrar, 7zip etc.", "New image saved !", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception) { throw; }
            finally
            {
                deleteDummies(tempDirectory);
            }

        }

        private void deleteDummies(string path)
        {
            DirectoryInfo di = new DirectoryInfo(@path);

            foreach (FileInfo file in di.GetFiles()) file.Delete();

            foreach (DirectoryInfo dir in di.GetDirectories()) dir.Delete(true);

            di.Delete(true);
        }

        private void encodeDecodeTextBoxText(bool encode)
        {
            try
            {
                string encodedText = new TextRange(rtxtTextToHide.Document.ContentStart, rtxtTextToHide.Document.ContentEnd).Text;
                encodedText = (encode) ? Gtools.encodeMix(encodedText,
                    MainWindow.username, MainWindow.password) :
                    Gtools.decodeMix(encodedText,
                    MainWindow.username, MainWindow.password);
                addTextToTextBox(encodedText);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void addTextToTextBox(string text)
        {
            try
            {
                rtxtTextToHide.Document.Blocks.Clear();
                FlowDocument ObjFdoc = new FlowDocument();
                Paragraph ObjPara1 = new Paragraph();
                ObjPara1.Inlines.Add(new Run(text));
                ObjFdoc.Blocks.Add(ObjPara1);
                rtxtTextToHide.Document = ObjFdoc;
            }
            catch (Exception)
            {

                throw;
            }
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

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            if (textTohide.Length < 1 || string.IsNullOrEmpty(imagePath))
            {
                string outPutString = (textTohide.Length < 1) ?
                    "Please add any text first." :
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

            }
            catch (Exception) { throw; }
        }

        private void expander_Expanded(object sender, RoutedEventArgs e)
        {
            this.Height += 100;
            parentWindow.changeHeight(this.Height + 50);
        }

        private void expander_Collapsed_1(object sender, RoutedEventArgs e)
        {
            this.Height -= 100;
            parentWindow.changeHeight(this.Height + 50);

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            rtxtTextToHide.Focus();
        }

        private void richTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textTohide = new TextRange(rtxtTextToHide.Document.ContentStart, rtxtTextToHide.Document.ContentEnd).Text;
        }

        private void btnResetImage_Click(object sender, RoutedEventArgs e)
        {
            image.Source = new BitmapImage(new Uri(@"pack://application:,,/Resources/Drop Image Here.png"));
        }

        private void btnEncode_Click(object sender, RoutedEventArgs e)
        {
            encodeDecodeTextBoxText(true);
        }

        private void btnDecode_Click(object sender, RoutedEventArgs e)
        {
            encodeDecodeTextBoxText(false);
        }

        private void btnLoadFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.DefaultExt = "*.txt";
                openFileDialog.Filter = "Text Files|*.txt";
                if (openFileDialog.ShowDialog() == true)
                {
                    string text = Gtools.readTextFromFile(openFileDialog.FileName);
                    addTextToTextBox(text);
                }
            }
            catch (Exception) { throw; }
        }

        private void btnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save your image as jpg file.";
            saveFileDialog.DefaultExt = "*.txt";
            saveFileDialog.Filter = "Text Files|*.txt";
            if (saveFileDialog.ShowDialog() == true)
                Gtools.createFile(saveFileDialog.FileName, textTohide);
        }

        private void rtxtTextToHide_PreviewDrop(object sender, DragEventArgs e)
        {
            try
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string file in files)
                {
                    if (System.IO.Path.GetExtension(file) == ".txt" || System.IO.Path.GetExtension(file) == ".TXT")
                    {
                        string text = Gtools.readTextFromFile(file);
                        addTextToTextBox(text);
                    }
                    else
                    {
                        MessageBox.Show("File is not valid text file." + Environment.NewLine + "Please insert a .txt file.",
                            "File format error!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                }
            }
            catch (Exception) { throw; }
        }

        private void rtxtTextToHide_PreviewDragEnter(object sender, DragEventArgs e)
        {
            e.Effects = (e.Data.GetDataPresent(DataFormats.FileDrop)) ? DragDropEffects.All : DragDropEffects.None;
        }

        #endregion
    }
}
