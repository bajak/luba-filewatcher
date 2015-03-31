using System;
using System.Globalization;
using System.Windows.Data;

namespace Filewatcher.GUI
{
    internal class EnumValueConverter : IValueConverter
    {
        private int _enumVal;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            _enumVal = (int)value;
            var enumMask = (int)parameter;
            return (_enumVal & enumMask) != 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolVal = (bool)value;
            var enumMask = (int)parameter;
            var contains = (_enumVal & enumMask) != 0;
            if (contains != boolVal)
                _enumVal = _enumVal ^ enumMask;
            return Enum.Parse(targetType, _enumVal.ToString(CultureInfo.InvariantCulture));
        }
    }
}
