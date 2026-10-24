using Microsoft.UI.Xaml.Data;
using System;

namespace vibe.Converters
{
    public class BoolToPlaybackStateIcons : IValueConverter
    {
        private const string PLAY_ICON = "\uF5B0";
        public const string PAUSE_ICON = "\uF8AE";

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not bool isPlaying)
            {
                throw new ArgumentException($"Le convertisseur {this} attend un booléan");
            }

            return isPlaying
                ? PAUSE_ICON
                : PLAY_ICON;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is string icon && icon == PLAY_ICON;
        }
    }
}
