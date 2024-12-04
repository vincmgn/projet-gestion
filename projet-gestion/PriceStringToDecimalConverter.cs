using System.Globalization;
using System.Windows.Data;

namespace projet_gestion
{
    public class PriceStringToDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return 0.0m;
            string strValue = value.ToString();
            strValue = strValue.Replace(",", ".");  // Remplacer la virgule par un point pour les formats européens
            if (decimal.TryParse(strValue, out decimal result))
                return result;
            return 0.0m;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();  // Pour convertir en chaîne lors de l'affichage
        }
    }
}