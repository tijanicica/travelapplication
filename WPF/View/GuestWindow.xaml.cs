using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.WPF.View.GuestPages;
using BookingApp.WPF.ViewModel.Guest;


namespace BookingApp.WPF.View
{
    public partial class GuestWindow : Window
    {
        private IAccommodationService _accommodationService;        

        public GuestWindow()
        {
            InitializeComponent();
         
            ActionBarName.Text = "Home Page"; // Postavite tekst u skladu sa trenutnom stranicom

            GetAllViewModel getAllViewModel = new GetAllViewModel(this);
            GetAllPage getAllPage = new GetAllPage();
            getAllPage.DataContext = getAllViewModel;
            MainFrameGuestWindow.Content = getAllPage;


            // MainFrameGuestWindow.Content = new HomePage(this, _accommodationService);
        }

        private void ToggleMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (ActionBar.Visibility == Visibility.Collapsed)
            {
                ActionBar.Visibility = Visibility.Visible;
            }
            else
            {
                ActionBar.Visibility = Visibility.Collapsed;
            }
        }

        private void HomePage_Click(object sender, RoutedEventArgs e)
        {
            /*MainFrameGuestWindow.Content = new HomePage(this, _accommodationService);
            MainFrameGuestWindow.Focus();*/
            GetAllViewModel getAllViewModel = new GetAllViewModel(this);
            GetAllPage getAllPage = new GetAllPage();
            getAllPage.DataContext = getAllViewModel;
            MainFrameGuestWindow.Content = getAllPage;
            ActionBar.Visibility = Visibility.Collapsed;
            ActionBarName.Text = "Home Page"; // Postavite tekst u skladu sa trenutnom stranicom


        }


        private void MyProfile_Click(object sender, RoutedEventArgs e)
        {
            
            MyProfileViewModel viewModel = new MyProfileViewModel(this);
            MyProfilePage myProfilePage = new MyProfilePage();
            myProfilePage.DataContext = viewModel;
            MainFrameGuestWindow.Content = myProfilePage;
            ActionBarName.Text = "My Profile"; // Postavite tekst u skladu sa trenutnom stranicom
            ActionBar.Visibility = Visibility.Collapsed;


        }
        
        
        private void ChangeReservationDates(object parameter)
        {
            var changeDatesPage = new ChangeReservationDatesPage();
            changeDatesPage.DataContext = DataContext; 
            MainFrameGuestWindow.Content = changeDatesPage;
            ActionBar.Visibility = Visibility.Collapsed;
        }
        
        

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                SignInForm signInForm = new SignInForm();
                signInForm.Show();
                this.Close();
            }
        }


        private bool _firstTimeOpened = true; // Dodajemo flag za praćenje prvog otvaranja notifikacija

        private void Notifications_Click(object sender, RoutedEventArgs e)
        {
            NotificationsViewModel viewModel = new NotificationsViewModel(this);

            // Ako je ovo prvi put otvaranja notifikacija, neka ostanu u sekciji "New"
            if (!_firstTimeOpened)
            {
                // Ako se klikne na opciju Notifikacije dok već postoje novi obaveštenja, očisti ih
                if (viewModel.NewNotifications.Any())
                {
                    viewModel.MarkNotificationsAsRead();
                }
            }
            else
            {
                // Ako je ovo prvi put otvaranja notifikacija, promenimo flag
                _firstTimeOpened = false;
            }

            NotificationsPage notificationsPage = new NotificationsPage();
            notificationsPage.DataContext = viewModel;
            MainFrameGuestWindow.Content = notificationsPage;

            ActionBar.Visibility = Visibility.Collapsed;
            ActionBarName.Text = "Notifications"; // Postavite tekst u skladu sa trenutnom stranicom
        }



        private void Forums_Click(object sender, RoutedEventArgs e)
        {
            ForumsViewModel viewModel = new ForumsViewModel(this);

            ForumsPage forumsPage = new ForumsPage();
            forumsPage.DataContext = viewModel;
            MainFrameGuestWindow.Content = forumsPage;
            ActionBar.Visibility = Visibility.Collapsed;     
            ActionBarName.Text = "Forums"; // Postavite tekst u skladu sa trenutnom stranicom


            
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            MainFrameGuestWindow.Content = new SearchForAccommodationPage(this, _accommodationService);
            ActionBar.Visibility = Visibility.Collapsed;
           ActionBarName.Text = "Search"; // Postavite tekst u skladu sa trenutnom stranicom

        }

        private void MyReservations_Click(object sender, RoutedEventArgs e)
        {
            MyReservationsViewModel viewModel = new MyReservationsViewModel(this);
            MyReservationsPage reservationsPage = new MyReservationsPage();
            reservationsPage.DataContext = viewModel;
            MainFrameGuestWindow.Content = reservationsPage;

            ActionBar.Visibility = Visibility.Collapsed;
            ActionBarName.Text = "My Reservations"; // Postavite tekst u skladu sa trenutnom stranicom

        }

        private void WW_Click(object sender, RoutedEventArgs e)
        {
            WheneverWhereverViewModel viewModel = new WheneverWhereverViewModel(this);

            WheneverWhereverPage wheneverWhereverPage = new WheneverWhereverPage();
            wheneverWhereverPage.DataContext = viewModel;
            MainFrameGuestWindow.Content = wheneverWhereverPage;
            ActionBar.Visibility = Visibility.Collapsed;
            ActionBarName.Text = "Whenever/wherever"; // Postavite tekst u skladu sa trenutnom stranicom

        }
 
        private void CloseActionBarButton_Click(object sender, RoutedEventArgs e)
        {
            ActionBar.Visibility = Visibility.Collapsed; // Sakrijemo ActionBar kada se klikne na dugme za zatvaranje
            ToggleMenuButton.IsChecked = false; // Postavimo ToggleMenuButton natrag na neoznačeno stanje
        }

    }
}