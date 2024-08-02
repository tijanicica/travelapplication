
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BookingApp.WPF.View.GuestPages.Converters

{
    public class SuperGuestToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isSuperGuest = (bool)value;
            return isSuperGuest ? Brushes.Gold : Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
