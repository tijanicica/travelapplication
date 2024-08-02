using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace BookingApp.WPF.Converters
{
    public class ImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string imagePath)
            {
                try
                {
                    // Dobijemo putanju do foldera gde se nalazi izvršni fajl (exe)
                    string executableFolderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                    // Nadovežemo relativnu putanju do foldera Resources/Images
                    string projectFolderPath = Directory.GetParent(executableFolderPath)?.Parent?.Parent?.FullName;
                    string imagesFolderPath = Path.Combine(projectFolderPath, "Resources", "Images");

                    string absolutePath = Path.Combine(imagesFolderPath, imagePath);

                    if (File.Exists(absolutePath))
                    {
                        // Kreiraj novi BitmapImage
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(absolutePath);
                        bitmap.EndInit();

                        Console.WriteLine($"Slika pronađena. Putanja: {absolutePath}");

                        return bitmap;
                    }
                    else
                    {
                        Console.WriteLine($"Slika nije pronađena. Putanja: {absolutePath}");
                        // U slučaju da slika nije pronađena, možete vratiti neku defaultnu sliku ili null
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Greška prilikom učitavanja slike: {ex.Message}");
                    return null;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
