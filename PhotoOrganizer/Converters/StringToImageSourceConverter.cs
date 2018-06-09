using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace PhotoOrganizer.Converters
{
    public class StringToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is string imageSourcePath))
                return DependencyProperty.UnsetValue;

            return new BitmapImage(new Uri($"ms-appdata:///{imageSourcePath}", UriKind.Absolute));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) => null;
    }
}