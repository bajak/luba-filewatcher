using System;

namespace Filewatcher.MDL
{
    public class ExitRequestEventArgs : EventArgs
    {
        public ExitRequestEventArgs(bool dialogResult, object value)
        {
            DialogResult = dialogResult;
            Value = value;
        }

        public bool DialogResult { get; private set; }
        public object Value { get; private set; }
    }
}
