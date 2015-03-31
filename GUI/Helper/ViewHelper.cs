using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Filewatcher.MDL;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace Filewatcher.GUI
{
    internal class ViewHelper
    {
        internal static Watch CreateDefaultWatch()
        {
            return new Watch
            {
                Name = ViewConfig.WATCH_DEFAULT_NAME,
                WatchFolder = ViewConfig.WATCH_DEFAULT_FOLDER,
                WorkingFolder = ViewConfig.WATCH_DEFAULT_FOLDER,
                IncludeExtension = ViewConfig.WATCH_DEFAULT_INCLUDEEXTENSION,
                ExcludeExtension = ViewConfig.WATCH_DEFAULT_EXCLUDEEXTENSION,
                Process = ViewConfig.WATCH_DEFAULT_PROCESS,
                Parameter = ViewConfig.WATCH_DEFAULT_PARAMETER,
                OutputPath = ViewConfig.WATCH_DEFAULT_OUTPUTPATH,
                Filter = ViewConfig.WATCH_DEFAULT_FILTER,
                IsProcessHidden = ViewConfig.WATCH_DEFAULT_ISPROCESSHIDDEN,
                IncludeSubfolders = ViewConfig.WATCH_DEFAULT_INCLUDESUBFOLDERS,
                LogOutput = ViewConfig.WATCH_DEFAULT_LOGOUTPUT,
                State = ViewConfig.WATCH_DEFAULT_STATE };
        }

        internal static Options CreateDefaultOptions()
        {
            return new Options { OutputPath = ViewConfig.OPTIONS_DEFAULT_OUTPUTPATH };
        }

        internal static string GetOpenFilePath(string ext, string filter, bool multiSelect)
        {
            var dialog = new OpenFileDialog {
                DefaultExt = ext, 
                Filter = filter, 
                Multiselect = multiSelect };

            return dialog.ShowDialog() == true ? dialog.FileName : string.Empty;
        }

        internal static string GetFolderPath(Environment.SpecialFolder initialFolder)
        {
            var dialog = new FolderBrowserDialog {
                RootFolder = initialFolder,
                ShowNewFolderButton = true };

            return dialog.ShowDialog() == DialogResult.OK ? dialog.SelectedPath : string.Empty;
        }

        internal static string GetSaveFilePath(string ext, string filter, string initialFolder)
        {
            var dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.AutoUpgradeEnabled = true;
            dialog.CheckPathExists = true;
            dialog.DefaultExt = ext;
            dialog.Filter = filter;
            dialog.InitialDirectory = initialFolder;

            return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : string.Empty;
        }

        internal static void OpenBrowser(string url)
        {
            var process = new Process {StartInfo = 
                    {UseShellExecute = true, FileName = url}
                };
            process.Start();
        }

        internal static void WriteToFile(string path, string text)
        {
            var streamWriter = new StreamWriter(path, true);
            streamWriter.Write(text);
            streamWriter.Close();
        }
    }
}
