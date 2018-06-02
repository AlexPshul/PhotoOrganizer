using System;
using System.Reactive;
using Windows.UI.Xaml.Data;

namespace PhotoOrganizer.Converters
{
    public class AnythingToUnitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => Unit.Default;
        public object ConvertBack(object value, Type targetType, object parameter, string language) => null;
    }
}