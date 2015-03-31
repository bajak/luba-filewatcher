using System.Collections.Generic;
using Filewatcher.MDL;

namespace Filewatcher.BLL
{
    public class Parameter
    {
        public static Dictionary<string, ParamMacro> Macros { get; private set; }

        static Parameter()
        {
            Macros = new Dictionary<string, ParamMacro>();

            var filePathMacro = new ParamMacro { Definition = "$TargetFilePath$" };
            filePathMacro.ReplaceFunc = new ParamMacroReplace(filePathMacro).ReplaceFilePath;
            Macros.Add(filePathMacro.Definition, filePathMacro);

            var filePathNoExtMacro = new ParamMacro { Definition = "$TargetFilePathNoExt$" };
            filePathNoExtMacro.ReplaceFunc = new ParamMacroReplace(filePathNoExtMacro).ReplaceFilePathNoExt;
            Macros.Add(filePathNoExtMacro.Definition, filePathNoExtMacro);

            var fileNameMacro = new ParamMacro { Definition = "$TargetFileName$" };
            fileNameMacro.ReplaceFunc = new ParamMacroReplace(fileNameMacro).ReplaceFileName;
            Macros.Add(fileNameMacro.Definition, fileNameMacro);

            var fileNameNoExtMacro = new ParamMacro { Definition = "$TargetFileNameNoExt$" };
            fileNameNoExtMacro.ReplaceFunc = new ParamMacroReplace(fileNameNoExtMacro).ReplaceFileNameNoExt;
            Macros.Add(fileNameNoExtMacro.Definition, fileNameNoExtMacro);

            var folderPathMacro = new ParamMacro { Definition = "$TargetFolderPath$" };
            folderPathMacro.ReplaceFunc = new ParamMacroReplace(folderPathMacro).ReplaceFolderPath;
            Macros.Add(folderPathMacro.Definition, folderPathMacro);

            var folderNameMacro = new ParamMacro { Definition = "$TargetFolderName$" };
            folderNameMacro.ReplaceFunc = new ParamMacroReplace(folderNameMacro).ReplaceFolderName;
            Macros.Add(folderNameMacro.Definition, folderNameMacro);
        }

        public static string Parse(string parameter, string targetPath)
        {
            foreach (var macro in Macros)
                parameter = macro.Value.ReplaceFunc(parameter, targetPath);
            return parameter;
        }
    }
}
