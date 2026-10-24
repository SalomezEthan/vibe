using Microsoft.UI.Xaml.Data;
using System;

namespace vibe.Converters
{
    public class TimeSpanToRoundedTimeSpan : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not TimeSpan time)
            {
                throw new ArgumentException("La valeur envoyé au convertisser {this} doit être un timespan");
            }

            var onSeconds = time.TotalSeconds;
            return TimeSpan.FromSeconds(Math.Round(onSeconds)).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
