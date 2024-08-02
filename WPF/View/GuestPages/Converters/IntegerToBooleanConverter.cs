using System;
using System.Globalization;
using System.Windows.Data;

namespace BookingApp.WPF.View.GuestPages.Converters
{
    public class IntegerToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            int intValue;
            int parameterValue;

            if (int.TryParse(value.ToString(), out intValue) && int.TryParse(parameter.ToString(), out parameterValue))
            {
                return intValue == parameterValue;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null || !(value is bool))
                return null;

            bool isChecked = (bool)value;
            int intValue = System.Convert.ToInt32(parameter);

            // Vratite vrednost na osnovu toga da li je RadioButton izabran ili ne
            return isChecked ? intValue : Binding.DoNothing;
        }

    }
}