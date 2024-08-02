using System;
using System.Globalization;
using System.Windows.Data;

namespace BookingApp.WPF.View.TouristPages;

public class IsAcceptedToButtonTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool isAccepted = value as bool? ?? false;
        return isAccepted ? "Confirmed" : "Confirm";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

