using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Autos_Accounts
{
    /// <summary>
    /// Returns true (enabled) when at least one bound bool value is true.
    /// Used in MainWindow to enable buttons only when a checkbox is checked.
    /// </summary>
    public class MultiCheckedToEnabledConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null) return false;
            return values.OfType<bool>().Any(b => b);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[0];
        }
    }
}
