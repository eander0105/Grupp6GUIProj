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
            ExitApplication();
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
            openFileDialog.Filter = "All files (*.*)|*.*";
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
                foreach (var item in files)
                {
                    Debug.WriteLine(item);
                }
                //string YourApplicationPath = "C:\\Users\\eande\\source\\repos\\Grupp6GUIProj\\Grupp6GUIProj\\molk";
                //ProcessStartInfo processInfo = new ProcessStartInfo();
                //processInfo.UseShellExecute = true;
                //processInfo.WindowStyle = ProcessWindowStyle.Hidden;
                //processInfo.FileName = "cmd.exe";
                //processInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(YourApplicationPath);
                //processInfo.Arguments = "/c molk C:\\Users\\eande\\Desktop\\Ny mapp\\test.molk " + files[0]/* + System.IO.Path.GetFileName(YourApplicationPath)*/;
                //Process.Start(processInfo);


                //var command = "molk";
                //System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);
                //procStartInfo.RedirectStandardOutput = true;
                //procStartInfo.UseShellExecute = false;
                //procStartInfo.WorkingDirectory = "C:\\Users\\eande\\source\\repos\\Grupp6GUIProj\\Grupp6GUIProj";
                //procStartInfo.CreateNoWindow = true; //whether you want to display the command window
                //System.Diagnostics.Process proc = new System.Diagnostics.Process();
                //proc.StartInfo = procStartInfo;
                //proc.Start();
                //string result = proc.StandardOutput.ReadToEnd();
                //Debug.WriteLine(result.ToString());

                //string strCmdText;
                //strCmdText = "";
                //System.Diagnostics.Process.Start("CMD.exe");

                Process cmd = new Process();

                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.RedirectStandardInput = true;
                cmd.StartInfo.RedirectStandardOutput = true;
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.UseShellExecute = false;
                cmd.Start();

                cmd.StandardInput.WriteLine(@"molk C:\Users\eande\Desktop\Ny-mapp\test.molk " + files[0]);
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.WaitForExit();
            }
        }
    }
}
