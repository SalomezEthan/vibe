using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vibe.Converters
{
    class BootToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not bool isVisible)
            {
                throw new ArgumentException($"Le convertisseur {this} attend un booléan");
            }

            if (parameter is string str && str.Equals("invert", StringComparison.CurrentCultureIgnoreCase))
            {
                isVisible = !isVisible;
            }

            return isVisible
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (Visibility)value is Visibility.Visible;
        }
    }
}
