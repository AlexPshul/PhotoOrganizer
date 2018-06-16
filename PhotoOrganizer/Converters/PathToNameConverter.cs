using System;
using System.IO;
using Windows.Graphics.Printing3D;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace PhotoOrganizer.Converters
{
    public class PathToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is string path))
                return DependencyProperty.UnsetValue;

            return Path.GetFileName(path);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) => null;
    }
}