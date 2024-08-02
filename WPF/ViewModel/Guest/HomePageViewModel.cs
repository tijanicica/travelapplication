using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;
using BookingApp.WPF.View.GuestPages;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class HomePageViewModel : ViewModelBase
    {
        private readonly IAccommodationReservationService _accommodationReservationService;
        private readonly IAccommodationService _accommodationService;
        private readonly GuestWindow _guestWindow;
        private ObservableCollection<AccommodationReservation> _reservations;
        private IReservationRescheduleService _reservationRescheduleService;


        public ObservableCollection<AccommodationReservation> Reservations
        {
            get => _reservations;
            set
            {
                _reservations = value;
                OnPropertyChanged(nameof(Reservations));
            }
        }

        public ICommand MyReservationsCommand { get; private set; }

        public HomePageViewModel()
        {
            _accommodationReservationService = Injector.Container.Resolve<IAccommodationReservationService>();
            _accommodationService = Injector.Container.Resolve<IAccommodationService>(); // Dodajemo injekciju za IAccommodationService
            _reservationRescheduleService = Injector.Container.Resolve<IReservationRescheduleService>();
            MyReservationsCommand = new RelayCommand(param => OpenGetMyReservations());
        }

        public HomePageViewModel(GuestWindow guestWindow) : this()
        {
            _guestWindow = guestWindow;
        }

        public void OpenGetMyReservations()
        {
            var app = Application.Current as App;
            int currentGuestId = app.LoggedUser.Id;

            Reservations = new ObservableCollection<AccommodationReservation>(_accommodationReservationService.GetAllReservationsByGuestId(currentGuestId));

            var myReservationsPage = new MyReservationsPage();
            myReservationsPage.DataContext = new MyReservationsViewModel(_guestWindow); // Prosleđujemo IAccommodationService
            _guestWindow.MainFrameGuestWindow.Content = myReservationsPage;

            //MessageBox.Show("DataContext set to MyReservationsViewModel!"); 
        }
    }
}
