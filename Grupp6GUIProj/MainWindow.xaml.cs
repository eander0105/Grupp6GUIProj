using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.ComponentModel;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Grupp6GUIProj {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you wish to close applicaton?", "Closing", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                ExitApplication();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            var res = MessageBox.Show("Do you wish to close application?", "Closing", MessageBoxButton.YesNo,MessageBoxImage.Exclamation);
            if (res == MessageBoxResult.Yes)
                base.OnClosing(e);
            else
            {
                e.Cancel = true;
            }
        }

        private void ExitApplication()
        {
            Application.Current.Shutdown();
        }



        private void StackPanel_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (var item in files)
                {
                    lbFiles.Items.Add(System.IO.Path.GetFileName(item));
                    Debug.WriteLine(item);
                }
                lbFilesContainer.Visibility = Visibility.Visible;
                DropFiles.Visibility = Visibility.Hidden;
            }
        }

        private void btnOpenFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            //openFileDialog.Filter = "All files (*.*)|*.*";
            openFileDialog.Filter = "All files (*.*)|*.*|Molk Archives (*.molk)|*.molk";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    lbFiles.Items.Add(filename);
                }
            }
            lbFilesContainer.Visibility = Visibility.Visible;
            DropFiles.Visibility = Visibility.Hidden;
        }


        private void btnOpenFiles_Click_Basic(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "All files (*.*)|*.*|Molk Archives (*.molk)|*.molk";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                string[] files = openFileDialog.FileNames;
                if (files.Length > 0)
                {
                    Process cmd = new Process();

                    cmd.StartInfo.FileName = "cmd.exe";
                    cmd.StartInfo.RedirectStandardInput = true;
                    cmd.StartInfo.RedirectStandardOutput = true;
                    cmd.StartInfo.CreateNoWindow = true;
                    cmd.StartInfo.UseShellExecute = false;
                    cmd.Start();
                    cmd.StandardInput.WriteLine("cd..\\..\\..\\");

                    if (files.Length == 1 && System.IO.Path.GetExtension(files[0]) == ".molk")
                    {
                        Debug.WriteLine(@"unmolk """ + files[0] + @""" -d """ + System.IO.Path.GetDirectoryName(files[0]) + @"""");
                        cmd.StandardInput.WriteLine(@"unmolk """ + files[0] + @""" -d """ + System.IO.Path.GetDirectoryName(files[0]) + @"""");
                    }
                    else
                    {
                        Debug.WriteLine("cd " + System.IO.Path.GetDirectoryName(files[0]));
                        cmd.StandardInput.Flush();
                        foreach (var item in files)
                        {
                            cmd.StandardInput.WriteLine(@"molk -r -j """ + System.IO.Path.GetDirectoryName(files[0]) + "\\" + System.IO.Path.GetFileNameWithoutExtension(files[0]) + @".molk"" """ + item + @"""");
                        }
                    }

                    cmd.StandardInput.Flush();
                    cmd.StandardInput.Close();
                    cmd.WaitForExit();
                    MessageBox.Show("File prossesing done", "Done", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.DefaultDesktopOnly);
                }


            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ClearlbFiles();
        }

        private void ClearlbFiles()
        {
            lbFiles.Items.Clear();
            lbFilesContainer.Visibility = Visibility.Hidden;
            DropFiles.Visibility = Visibility.Visible;
        }

        private void StackPanel_Drop_Basic(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0)
                {
                    Process cmd = new Process();

                    cmd.StartInfo.FileName = "cmd.exe";
                    cmd.StartInfo.RedirectStandardInput = true;
                    cmd.StartInfo.RedirectStandardOutput = true;
                    cmd.StartInfo.CreateNoWindow = true;
                    cmd.StartInfo.UseShellExecute = false;
                    cmd.Start();
                    cmd.StandardInput.WriteLine("cd..\\..\\..\\");

                    if (files.Length == 1 && System.IO.Path.GetExtension(files[0]) == ".molk")
                    {
                        Debug.WriteLine(@"unmolk """ + files[0] + @""" -d """ + System.IO.Path.GetDirectoryName(files[0]) + @"""");
                        cmd.StandardInput.WriteLine(@"unmolk """ + files[0] + @""" -d """ + System.IO.Path.GetDirectoryName(files[0]) + @"""");
                    }
                    else
                    {
                        Debug.WriteLine("cd " + System.IO.Path.GetDirectoryName(files[0]));
                        cmd.StandardInput.Flush();
                        foreach (var item in files)
                        {
                            cmd.StandardInput.WriteLine(@"molk -r -j """ + System.IO.Path.GetDirectoryName(files[0]) + "\\" + System.IO.Path.GetFileNameWithoutExtension(files[0]) + @".molk"" """ + item + @"""");
                        }
                    }

                    cmd.StandardInput.Flush();
                    cmd.StandardInput.Close();
                    cmd.WaitForExit();
                    MessageBox.Show("File prossesing done", "Done", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.DefaultDesktopOnly);
                }

            }
        }

        private void UnMolkFileLocation(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog folderDialog = new CommonOpenFileDialog();
            folderDialog.InitialDirectory = "C:\\Users";
            folderDialog.IsFolderPicker = true;
            if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                MessageBox.Show("You selected: " + folderDialog.FileName);
            }
            Process cmd = new Process();

            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            cmd.StandardInput.WriteLine("cd..\\..\\..\\");
            try
            {
                cmd.StandardInput.WriteLine(@"unmolk """ + pathList[0] + @""" -d """ + folderDialog.FileName + @"""");
            }
            catch
            {
                MessageBox.Show("could not unmolk the file please try again", "Could not unmolk", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.DefaultDesktopOnly);
            }
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            MessageBox.Show("File prossesing done", "Done", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.DefaultDesktopOnly);

        }

        private void MolkFileLocation(object sender, RoutedEventArgs e)
        {

        }
    }
}
