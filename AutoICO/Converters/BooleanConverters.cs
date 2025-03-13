using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AutoICO.Converters
{
    /// <summary>
    /// Converts a boolean value to a visibility value.
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = value is bool b && b;
            bool invert = parameter is string s && s.ToLower() == "invert";
            
            if (invert)
            {
                boolValue = !boolValue;
            }
            
            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool invert = parameter is string s && s.ToLower() == "invert";
            bool result = value is Visibility v && v == Visibility.Visible;
            
            return invert ? !result : result;
        }
    }

    /// <summary>
    /// Converts a boolean value to its inverse.
    /// </summary>
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                return !b;
            }
            
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                return !b;
            }
            
            return value;
        }
    }
} 