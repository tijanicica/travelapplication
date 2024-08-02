using System.Windows;
using System.Windows.Controls;
using BookingApp.Appl.UseCases;
using BookingApp.WPF.ViewModel.Guest;

namespace BookingApp.WPF.View.GuestPages
{
    public partial class HomePage : Page
    {
        private GuestWindow _guestWindow;
        private readonly IAccommodationService _accommodationService;
        

        public HomePage(GuestWindow guestWindow, IAccommodationService accommodationService)
        {
            InitializeComponent();
            _guestWindow = guestWindow;
            _accommodationService = accommodationService; 
            DataContext = new HomePageViewModel(); // Inicijalizujte DataContext na MyReservationsViewModel

            
           // var app = Application.Current as App;
            //Console.WriteLine(app.LoggedUser.Id);
        }

        private void OpenGetAllPage(object sender, RoutedEventArgs e)
        {
            if (_guestWindow != null)
            {
                _guestWindow.MainFrameGuestWindow.Content = new GetAllPage();
            }
            else
            {
                MessageBox.Show("GuestWindow is not set.");
            }
        }

        private void OpenSearchForAccommodation(object sender, RoutedEventArgs e)
        {
            if (_guestWindow != null)
            {
                _guestWindow.MainFrameGuestWindow.Content = new SearchForAccommodationPage(_guestWindow, _accommodationService);
            }
            else
            {
                MessageBox.Show("GuestWindow or AccommodationController is not set.");
            }   
        }


        private void OpenGetMyReservations(object sender, RoutedEventArgs e)
        {
            if (DataContext is HomePageViewModel viewModel)
            {
                viewModel.OpenGetMyReservations();
            }
        }



    }
}