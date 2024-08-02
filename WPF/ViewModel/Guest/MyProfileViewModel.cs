using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Service;
using BookingApp.Utils;
using BookingApp.WPF.View;
using BookingApp.WPF.View.GuestPages;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class MyProfileViewModel : ViewModelBase
    {
        private string _username;
        private GuestWindow _guestWindow;
        private IUserService _userService;
        private ISuperGuestService _superGuestService;

        public ObservableCollection<SuperGuest> SuperGuests { get; set; }

        public ICommand ReserveCommand { get; private set; }
        public ICommand SignOutCommand { get; private set; }

        public string Username
        {
            get { return _username; }
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        private string _superGuest;
        public string SuperGuest
        {
            get { return _superGuest; }
            set
            {
                if (_superGuest != value)
                {
                    _superGuest = value;
                    OnPropertyChanged(nameof(SuperGuest));
                }
            }
        }
        
        private bool _isSuperGuest;
        public bool IsSuperGuest
        {
            get { return _isSuperGuest; }
            set
            {
                if (_isSuperGuest != value)
                {
                    _isSuperGuest = value;
                    OnPropertyChanged(nameof(IsSuperGuest));
                }
            }
        }

        private int _numberOfPoints;
        public int NumberOfPoints
        {
            get { return _numberOfPoints; }
            set
            {
                if (_numberOfPoints != value)
                {
                    _numberOfPoints = value;
                    OnPropertyChanged(nameof(NumberOfPoints));
                }
            }
        }

        public MyProfileViewModel(GuestWindow guestWindow)
        {
            _guestWindow = guestWindow;
            _userService = Injector.Container.Resolve<IUserService>();
            _superGuestService = Injector.Container.Resolve<ISuperGuestService>();
            var app = System.Windows.Application.Current as App;
            Username = _userService.GetUsernameById(app.LoggedUser.Id);
            IsSuperGuest = _superGuestService.IsUserSuperGuest(app.LoggedUser.Id);
            _superGuestService.GetSuperGuests(app.LoggedUser);
            var superGuest = _superGuestService.GetByGuestId(app.LoggedUser.Id);
            SuperGuests = new ObservableCollection<SuperGuest>();
            if (superGuest != null)
            {
                SuperGuests.Add(superGuest);
                NumberOfPoints = superGuest.BonusPoints; // Set NumberOfPoints here
            }
            ReserveCommand = new RelayCommand(ShowReservations);
            SignOutCommand = new RelayCommand(SignOut);
        }

        public void SignOut(object parameter)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                SignInForm signInForm = new SignInForm();
                signInForm.Show();
                _guestWindow.Close();
            }
        }

        public void ShowReservations(object parameter)
        {
            MyReservationsViewModel viewModel = new MyReservationsViewModel(_guestWindow);
            MyReservationsPage reservationsPage = new MyReservationsPage();
            reservationsPage.DataContext = viewModel;
            _guestWindow.MainFrameGuestWindow.Content = reservationsPage;
        }
    }
}
