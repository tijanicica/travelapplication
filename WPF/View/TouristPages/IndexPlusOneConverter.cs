using System;
using System.Globalization;
using System.Windows.Data;

namespace BookingApp.WPF.View.TouristPages;

public class IndexPlusOneConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int index)
            return index + 1;  // Add one to the index
        return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();  // Not needed for this use case
    }
}