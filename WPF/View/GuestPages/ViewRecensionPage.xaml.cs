using System.Windows;
using System.Windows.Controls;

namespace BookingApp.WPF.View.GuestPages;

public partial class ViewRecensionPage : Page
{
    public ViewRecensionPage()
    {
        InitializeComponent();
    }

    
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }    
}