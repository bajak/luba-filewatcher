using System;

namespace Filewatcher.MDL
{
    public class ParamMacro
    {
        public string Definition { get; set; }
        public string ViewHead { get; set; }
        public string ViewText { get; set; }
        public Func<string, string, string> ReplaceFunc { get; set; }
    }
}
