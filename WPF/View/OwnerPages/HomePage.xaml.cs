using System.Windows;
using System.Windows.Controls;

namespace BookingApp.WPF.View.OwnerPages;

public partial class HomePage : Page
{
    private OwnerWindow _ownerWindow;
    public HomePage()
    {
        InitializeComponent();
       // _ownerWindow = new OwnerWindow();
        this.DataContext = this;
       // _ownerWindow = ownerWindow;
    }

    private void OpenCreateAccommodationPage(object sender, RoutedEventArgs e)
    {
        this.NavigationService?.Navigate(new CreateAccommodationPage(_ownerWindow));
      //  _ownerWindow.MainFrameOwnerWindow.Content = new CreateAccommodationPage(_ownerWindow);
    }

    private void OpenGuestReviewPage(object sender, RoutedEventArgs e)
    {
        this.NavigationService?.Navigate(new FilteredGuestsPage());
        //_ownerWindow.MainFrameOwnerWindow.Content = new AllGuestsPage(_ownerWindow);
    }

 
}