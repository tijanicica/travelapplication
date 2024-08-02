using System.Globalization;
using System.Windows.Controls;

namespace BookingApp.WPF.View.GuestPages
{
    public class NumericValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            // Proveravamo da li je vrednost null ili prazna
            if (string.IsNullOrEmpty(value as string))
            {
                // Ako je vrednost null ili prazna, smatramo da je validna
                return ValidationResult.ValidResult;
            }

            // Proveravamo da li je vrednost numerička
            int result;
            if (int.TryParse(value as string, out result))
            {
                // Ako je vrednost numerička, smatramo da je validna
                return ValidationResult.ValidResult;
            }
            else
            {
                // Ako vrednost nije numerička, vraćamo grešku
                return new ValidationResult(false, "Please enter a valid number.");
            }
        }
    }
}