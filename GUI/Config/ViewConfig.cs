using System;
using System.IO;
using System.Reflection;
using Filewatcher.BLL;
using P = Filewatcher.GUI.Properties.Resources;
using Filter = System.IO.NotifyFilters;
using Folder = System.Environment.SpecialFolder;
using State = Filewatcher.MDL.WatchState;

namespace Filewatcher.GUI
{
    internal static class ViewConfig
    {
        static ViewConfig()
        {
            InitializeMacroViewProperties();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //WATCH VIEW////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static string    WATCH_DEFAULT_NAME = P.WATCH_DEFAULT_NAME.Alt("Default Name");
        public static string    WATCH_DEFAULT_FOLDER = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string    WATCH_DEFAULT_INCLUDEEXTENSION = "*.txt";
        public static string    WATCH_DEFAULT_EXCLUDEEXTENSION = "";
        public static string    WATCH_DEFAULT_PROCESS = Environment.SystemDirectory + @"\cmd.exe";
        public static string    WATCH_DEFAULT_PARAMETER = P.WATCH_DEFAULT_PARAMETER.Alt("/T:C0 /K Echo File access on '$TargetFileName$'");
        public static string    WATCH_DEFAULT_OUTPUTPATH = "";
        public static Filter    WATCH_DEFAULT_FILTER = NotifyFilters.LastWrite;
        public static State     WATCH_DEFAULT_STATE = State.Enabled;
        public static bool      WATCH_DEFAULT_INCLUDESUBFOLDERS = true;
        public static bool      WATCH_DEFAULT_LOGOUTPUT = true;
        public static bool      WATCH_DEFAULT_ISPROCESSHIDDEN = false;
        public static string    WATCH_GENERIC_OPENFILE_ALLFILES = P.WATCH_GENERIC_ALLFILES.Alt("All Files") + " (*.*)|*.*";
        public static string    WATCH_GENERIC_OPENFILE_DEF_EXTENSION = "*.*";
        public static Folder    WATCH_GENERIC_OPENFOLDER_ROOT = Environment.SpecialFolder.MyComputer;
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //MACRO VIEW////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static void InitializeMacroViewProperties() {
            Parameter.Macros["$TargetFilePath$"].ViewHead = P.MACRO_LABEL_TITLE_FILEPATH;
            Parameter.Macros["$TargetFilePath$"].ViewText = P.MACRO_LABEL_TEXT_FILEPATH;

            Parameter.Macros["$TargetFilePathNoExt$"].ViewHead = P.MACRO_LABEL_TITLE_FILEPATHNOEXT;
            Parameter.Macros["$TargetFilePathNoExt$"].ViewText = P.MACRO_LABEL_TEXT_FILEPATHNOEXT;

            Parameter.Macros["$TargetFileName$"].ViewHead = P.MACRO_LABEL_TITLE_FILENAME;
            Parameter.Macros["$TargetFileName$"].ViewText = P.MACRO_LABEL_TEXT_FILENAME;

            Parameter.Macros["$TargetFileNameNoExt$"].ViewHead = P.MACRO_LABEL_TITLE_FILENAMENOEXT;
            Parameter.Macros["$TargetFileNameNoExt$"].ViewText = P.MACRO_LABEL_TEXT_FILENAMENOEXT;

            Parameter.Macros["$TargetFolderPath$"].ViewHead = P.MACRO_LABEL_TITLE_FOLDERPATH;
            Parameter.Macros["$TargetFolderPath$"].ViewText = P.MACRO_LABEL_TEXT_FOLDERPATH;

            Parameter.Macros["$TargetFolderName$"].ViewHead = P.MACRO_LABEL_TITLE_FOLDERNAME;
            Parameter.Macros["$TargetFolderName$"].ViewText = P.MACRO_LABEL_TEXT_FOLDERNAME;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //MAIN VIEW/////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static string    OPTIONS_DEFAULT_OUTPUTPATH = Properties.Settings.Default.SAVEOUTPUTLOCATION.Alt(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\output.txt");
        public static string    OPTIONS_DEFAULT_OUTPUTPATH_INITIALPATH = Assembly.GetExecutingAssembly().Location;
        public static string    OPTIONS_DEFAULT_OUTPUTPATH_EXTENSION = "*.txt";
        public static string    OPTIONS_DEFAULT_OUTPUTPATH_FILTER = P.MAIN_OPTIONS_OUTPUT_TEXTFILES.Alt("Text Files") + " (*.txt)|*.txt";
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //SPLASH VIEW///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static bool      SPLASH_USETIMER = true;
        public static int       SPLASH_TIMERDURATION = 6;
        public static string    SPLASH_DEFAULTURL = "http://www.bajak.net";
        public static string    SPLASH_DONATEURL = "https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=A62VD6RDNKVHU";
    }
}
