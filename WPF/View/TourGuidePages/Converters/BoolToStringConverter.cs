using System;
using System.Globalization;
using System.Windows.Data;

namespace BookingApp.WPF.View.TourGuidePages.Converters;

public class BoolToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (!(value is bool))
            return value;

        bool isValid = (bool)value;

        return isValid ? "Valid" : "Invalid";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}