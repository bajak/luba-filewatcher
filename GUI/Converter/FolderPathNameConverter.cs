using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace Filewatcher.GUI
{
    internal class FolderPathNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string))
                return null;
            return new DirectoryInfo(value.ToString()).Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
