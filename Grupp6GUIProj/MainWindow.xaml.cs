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
using System.Windows.Threading;

namespace Grupp6GUIProj {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public List<string> pathList = new List<string>();
        private DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();
            _timer = new DispatcherTimer();
            _timer.Tick += timer_Tick;
            _timer.Interval = new System.TimeSpan(0, 0, 7);
        }
        private void timer_Tick(object sender, System.EventArgs e)
        {
            Debug.WriteLine("no");
            ProgressBasic.Visibility = Visibility.Hidden;
            ProgressAdvanced.Visibility = Visibility.Hidden;
            _timer.Stop();
            MessageBox.Show("File prossesing done", "Done", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.DefaultDesktopOnly);
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
            var res = MessageBox.Show("Do you wish to close application?", "Closing", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
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

        private void StackPanel_Drop_Advnaced(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (var item in files)
                {
                    pathList.Add(item);
                    lbFiles.Items.Add(System.IO.Path.GetFileName(item));
                }
                if (pathList.Count() == 1)
                {
                    if (System.IO.Path.GetExtension(pathList[0]) == ".molk")
                    {
                        MolkFileBtn.IsEnabled = false;
                        UnMolkFileBtn.IsEnabled = true;
                    }
                    else
                    {
                        MolkFileBtn.IsEnabled = true;
                        UnMolkFileBtn.IsEnabled = false;
                    }
                }
                else if (pathList.Count() > 1)
                {
                    MolkFileBtn.IsEnabled = true;
                    UnMolkFileBtn.IsEnabled = false;
                }
                lbFilesContainer.Visibility = Visibility.Visible;
                DropFiles.Visibility = Visibility.Hidden;
            }
        }

        private void btnOpenFiles_Click_Advanced(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "All files (*.*)|*.*|Molk Archives (*.molk)|*.molk";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    pathList.Add(filename);
                    lbFiles.Items.Add(System.IO.Path.GetFileName(filename));
                }
                if (pathList.Count() == 1)
                {
                    if (System.IO.Path.GetExtension(pathList[0]) == ".molk")
                    {
                        MolkFileBtn.IsEnabled = false;
                        UnMolkFileBtn.IsEnabled = true;
                    }
                    else
                    {
                        MolkFileBtn.IsEnabled = true;
                        UnMolkFileBtn.IsEnabled = false;
                    }
                }
                else if (pathList.Count() > 1)
                {
                    MolkFileBtn.IsEnabled = true;
                    UnMolkFileBtn.IsEnabled = false;
                }
                lbFilesContainer.Visibility = Visibility.Visible;
                DropFiles.Visibility = Visibility.Hidden;
            }
            
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
                        cmd.StandardInput.WriteLine(@"unmolk """ + files[0] + @""" -d """ + System.IO.Path.GetDirectoryName(files[0]) + @"""");
                        Process.Start("explorer.exe", System.IO.Path.GetDirectoryName(files[0]));

                    }
                    else
                    {
                        Debug.WriteLine("cd " + System.IO.Path.GetDirectoryName(files[0]));
                        cmd.StandardInput.Flush();
                        foreach (var item in files)
                        {
                            cmd.StandardInput.WriteLine(@"molk -r -j """ + System.IO.Path.GetDirectoryName(files[0]) + "\\" + System.IO.Path.GetFileNameWithoutExtension(files[0]) + @".molk"" """ + item + @"""");
                            Process.Start("explorer.exe", System.IO.Path.GetDirectoryName(files[0]));
                        }
                    }

                    cmd.StandardInput.Flush();
                    cmd.StandardInput.Close();
                    cmd.WaitForExit();
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
            pathList.Clear();

            MolkFileBtn.IsEnabled = false;
            UnMolkFileBtn.IsEnabled = false;
            FileName.IsEnabled = false;
            TextFileField.IsEnabled = false;

            lbFilesContainer.Visibility = Visibility.Hidden;
            DropFiles.Visibility = Visibility.Visible;
        }

        private void StackPanel_Drop_Basic(object sender, DragEventArgs e)
        {
            ProgressBasic.Visibility = Visibility.Visible;
            _timer.Start();
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
                    //MessageBox.Show("File prossesing done", "Done", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.DefaultDesktopOnly);
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
                    Process.Start("explorer.exe", folderDialog.FileName);
                }
                catch
                {
                    MessageBox.Show("Could not unmolk the file please try again", "Could not unmolk", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.DefaultDesktopOnly);
                }
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.WaitForExit();

                ClearlbFiles();
            }
        }

        private void MolkFileLocation(object sender, RoutedEventArgs e)
        {         
            CommonOpenFileDialog folderDialog = new CommonOpenFileDialog();
            folderDialog.InitialDirectory = "C:\\Users";
            folderDialog.IsFolderPicker = true;
            if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                MessageBox.Show("You selected: " + folderDialog.FileName);

                Process cmd = new Process();

                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.RedirectStandardInput = true;
                cmd.StartInfo.RedirectStandardOutput = true;
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.UseShellExecute = false;
                cmd.Start();
                cmd.StandardInput.WriteLine("cd..\\..\\..\\");

                ProgressAdvanced.Visibility = Visibility.Visible;
                _timer.Start();

                cmd.StandardInput.Flush();
                foreach (var item in pathList)
                {
                    try
                    {
                        cmd.StandardInput.WriteLine(@"molk -r -j """ + folderDialog.FileName + "\\" + System.IO.Path.GetFileNameWithoutExtension(pathList[0]) + @".molk"" """ + item + @"""");
                    }
                    catch
                    {
                        MessageBox.Show("could not molk  " + item + "please try again", "Could not molk", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.DefaultDesktopOnly);

                    }
                }
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.WaitForExit();
               

            }

        }

        private void lbFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbFiles.SelectedItem != null)
            {
                FileName.Text = lbFiles.SelectedItem.ToString();
                if (System.IO.Path.GetExtension(lbFiles.SelectedItem.ToString()) == ".txt")
                {
                    var text = System.IO.File.ReadAllLines(pathList[lbFiles.SelectedIndex]);
                    TextFileField.Text = "";
                    foreach (var item in text)
                    {
                        TextFileField.Text += item + "\n";
                    }
                    FileName.IsEnabled = true;
                    TextFileField.IsEnabled = true;
                }
                else
                {
                    TextFileField.Text = "";
                    FileName.IsEnabled = true;
                    TextFileField.IsEnabled = false;
                }
            }
            else
            {
                FileName.Text = "";
            }           
        }
    }
}
