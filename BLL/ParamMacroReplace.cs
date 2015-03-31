using System.IO;
using Filewatcher.MDL;

namespace Filewatcher.BLL
{
    internal class ParamMacroReplace
    {
        private readonly ParamMacro _macro;

        public ParamMacroReplace(ParamMacro macro)
        {
            _macro = macro;
        }

        public string ReplaceFilePath(string parameterText, string targetFilePath)
        {
            return parameterText.Replace(_macro.Definition, targetFilePath);
        }

        public string ReplaceFilePathNoExt(string parameterText, string targetFilePath)
        {
            return parameterText.Replace(_macro.Definition, 
                Path.Combine(Path.GetDirectoryName(targetFilePath), 
                             Path.GetFileNameWithoutExtension(targetFilePath)));
        }

        public string ReplaceFileName(string parameterText, string targetFilePath)
        {
            return parameterText.Replace(_macro.Definition, Path.GetFileName(targetFilePath));
        }

        public string ReplaceFileNameNoExt(string parameterText, string targetFilePath)
        {
            return parameterText.Replace(_macro.Definition, Path.GetFileNameWithoutExtension(targetFilePath));
        }

        public string ReplaceFolderPath(string parameterText, string targetFilePath)
        {
            return parameterText.Replace(_macro.Definition, Path.GetDirectoryName(targetFilePath));
        }

        public string ReplaceFolderName(string parameterText, string targetFilePath)
        {
            return parameterText.Replace(_macro.Definition, 
                new DirectoryInfo(Path.GetDirectoryName(targetFilePath)).Name);
        }
    }
}
