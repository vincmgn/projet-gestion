using System;
using System.Globalization;
using System.Windows.Data;

namespace projet_gestion
{
    public class NullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? null : Binding.DoNothing;
        }
    }
}


