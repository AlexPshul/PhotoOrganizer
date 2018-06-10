using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace PhotoOrganizer.Converters
{
    public class SubtractionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!double.TryParse(value.ToString(), out double number))
                return DependencyProperty.UnsetValue;

            if (!double.TryParse(parameter.ToString(), out double substraction))
                return DependencyProperty.UnsetValue;

            double result = number - substraction;
            return result > 0 ? result : DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}