using System.Windows;
using BookingApp.WPF.View.TouristPages;
using BookingApp.WPF.ViewModel.Tourist;
using HomePage = BookingApp.WPF.View.TouristPages.HomePage;

namespace BookingApp.WPF.View;

public partial class TouristWindow : Window
{
    public TouristWindow()
    {
        InitializeComponent();
        MainFrame.Content = new HomePage();
    }


    private void LogoButton_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Content = new HomePage();
    }

    private void RequestTour_Click(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void MyRequests_Click(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void MyTours_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Content = new MyToursPage()
        {
            DataContext = new MyToursViewModel(this)
        };
    }

    private void MyVouchers_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Content = new MyVouchers()
        {
            DataContext = new MyVouchersViewModel()
        };
    }

    private void Notifications_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Content = new NotificationsPage()
        {
            DataContext = new NotificationsViewModel(this)
        };
    }

    private void LogOut_Click(object sender, RoutedEventArgs e)
    {
        OverlayGridLogOut.Visibility = Visibility.Visible;
    }

    private void YesButtonLogOut_Click(object sender, RoutedEventArgs e)
    {
        // Korisnik je potvrdio odjavu
        SignInForm signInForm = new SignInForm();
        signInForm.Show();
        this.Close();
    }

    private void NoButtonLogOut_Click(object sender, RoutedEventArgs e)
    {
        // Korisnik je odbio odjavu, sakrij overlay grid
        OverlayGridLogOut.Visibility = Visibility.Collapsed;
    }

    private void MyReservations_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Content = new MyReservationsPage()
        {
            DataContext = new MyReservationsViewModel(this)
        };
    }

    private void RegularTour_Click(object sender, RoutedEventArgs e)
    {
        
        MainFrame.Content = new RegularTourRequestPage()
        {
            DataContext = new RegularTourRequestViewModel(this)
        };
    }

    private void ComplexTour_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Content = new ComplexTourRequestPage()
        {
            DataContext = new ComplexTourRequestViewModel(this)
        };
    }

    private void RegularTourView_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Content = new MyRequestRegularTourPage()
        {
            DataContext = new MyRequestsRegularTourViewModel(this)
        };
    }

    private void ComplexTourView_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Content = new MyComplexTourPage()
        {
            DataContext = new MyComplexTourRequestViewModel(this)
        };
    }

    private void NewTours_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Content = new NewToursPage()
        {
            DataContext = new NewToursViewModel(this)
        };
    }
}