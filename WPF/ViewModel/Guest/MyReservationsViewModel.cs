using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;
using BookingApp.WPF.View.GuestPages;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class MyReservationsViewModel : ViewModelBase
    {
        private ObservableCollection<AccommodationReservation> _reservations;
        private IAccommodationReservationService _accommodationReservationService;
        private IAccommodationService _accommodationService;
        private GuestWindow _guestWindow;
        private  IReservationRescheduleService _reservationRescheduleService;
        private  IOwnerNotificationService _ownerNotificationService;
        private IGuestReviewService _guestReviewService;

        public MyReservationsViewModel(GuestWindow guestWindow)
        {

            InitializeServices();
            _guestWindow = guestWindow;
            Initialize();
            InitializeCommands();
        }
        private void InitializeServices()
        {
            _accommodationReservationService = Injector.Container.Resolve<IAccommodationReservationService>();
            _accommodationService = Injector.Container.Resolve<IAccommodationService>();
            _reservationRescheduleService = Injector.Container.Resolve<IReservationRescheduleService>();
            _ownerNotificationService = Injector.Container.Resolve<IOwnerNotificationService>();
            _guestReviewService = Injector.Container.Resolve<IGuestReviewService>();
        }
        private void InitializeCommands()
        {
            ChangeDatesCommand = new DelegateCommand(ChangeDates);
            SelectionChangedCommand = new DelegateCommand(ComboBoxSelectionChanged);
            RemoveReservationCommand = new DelegateCommand(RemoveReservation);
            RateAccommodationCommand = new DelegateCommand(Rate);
            ViewRecensionsCommand = new DelegateCommand(ViewRecensions);
        }

        private void ChangeDates(object parameter)
        { 
            if (parameter is AccommodationReservation selectedReservation)
            {
                ChangeReservationDatesViewModel viewModel = new ChangeReservationDatesViewModel(selectedReservation,this,  _guestWindow);

                ChangeReservationDatesPage changeDatesPage = new ChangeReservationDatesPage();
                changeDatesPage.DataContext = viewModel;

                _guestWindow.MainFrameGuestWindow.Content = changeDatesPage;
            }
            else
            {
                MessageBox.Show("Invalid parameter type or parameter is null.");
            }
        }
        

        
        public ObservableCollection<AccommodationReservation> Reservations
        {
            get { return _reservations; }
            set
            {
                _reservations = value;
                OnPropertyChanged(nameof(Reservations));
            }
        }
        
        public ICommand ChangeDatesCommand { get; private set; }
        public ICommand RemoveReservationCommand { get; private set; }
        
        public ICommand ViewRecensionsCommand { get; private set; }

        public void Initialize()
        {
            var app = Application.Current as App;
            if (app == null || app.LoggedUser == null)
            {
                MessageBox.Show("Logged user is null.");
                return;
            }

            SelectedReservationType = "Current reservations";
            UpdateReservations(SelectedReservationType);
        }

        public ICommand SelectionChangedCommand { get; set; }

        private string _selectedReservationType;

        public string SelectedReservationType
        {
            get { return _selectedReservationType; }
            set
            {
                _selectedReservationType = value;
                UpdateReservations(value);
                OnPropertyChanged(nameof(SelectedReservationType));
            }
        }
        
        
        
        public void UpdateReservations(string selectedType)
        {
            var app = Application.Current as App;
            var guestId = app.LoggedUser.Id;
            switch (selectedType)
            {
                case "Current reservations":
                    Reservations = new ObservableCollection<AccommodationReservation>(
                        _accommodationReservationService.GetReservationsNotInOtherListsAndNotInReschedule(guestId)
                    );
                    break;
                case "Approved reservations":
                    Reservations = new ObservableCollection<AccommodationReservation>(
                        _accommodationReservationService.GetApprovedReservations(guestId)
                    );
                    break;
                case "Pending reservations":
                    Reservations = new ObservableCollection<AccommodationReservation>(
                        _accommodationReservationService.GetPendingReservations(guestId)
                    );
                    break;
                case "Rejected reservations":
                    Reservations = new ObservableCollection<AccommodationReservation>(
                        _accommodationReservationService.GetRejectedReservations(guestId)
                    );
                    break;
                case "Previous reservations":
                    Reservations = new ObservableCollection<AccommodationReservation>(
                        _accommodationReservationService.GetPreviousReservations(guestId)
                    );
                    break;
                default:
                    break;
            }
        }

        private void ComboBoxSelectionChanged(object parameter)
        {
            if (parameter is ComboBox comboBox)
            {
                if (comboBox.SelectedItem != null)
                {
                    if (comboBox.SelectedItem is ComboBoxItem selectedItem)
                    {
                        string selectedType = selectedItem.Content as string;
                        if (selectedType != null)
                        {
                            UpdateReservations(selectedType);
                            OnPropertyChanged(nameof(SelectedReservationType));
                        }
                        else
                        {
                            MessageBox.Show("Content of the selected item is not a string");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("SelectedItem is null");
                }
            }
        }


        private void RemoveReservation(object parameter)
        {
            var app = Application.Current as App;
            if (app == null || app.LoggedUser == null)
            {
                MessageBox.Show("Logged user is null.");
                return;
            }

            if (parameter is not AccommodationReservation selectedReservation)
                return;

            if (MessageBox.Show("Are you sure you want to remove this reservation?", "Confirmation", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;

            var accommodation = _accommodationService.GetAccommodationById(selectedReservation.AccommodationId);

            if (accommodation == null)
            {
                MessageBox.Show("Accommodation not found.");
                return;
            }

            bool canCancel = false;

            if (accommodation.CancellationDeadline.HasValue)
            {
                DateTime cancellationDeadline = selectedReservation.StartDate.AddDays(-accommodation.CancellationDeadline.Value);
                canCancel = DateTime.Now < cancellationDeadline;
            }
            else
            {
                canCancel = selectedReservation.StartDate > DateTime.Now.AddDays(1);
            }

            if (!canCancel)
            {
                MessageBox.Show("It is not possible to cancel this reservation at this time.");
                return;
            }

            _accommodationReservationService.RemoveReservation(selectedReservation.Id);
            UpdateReservations(SelectedReservationType);

            OwnerNotification notification = new OwnerNotification()
            {
                ReservationId = selectedReservation.Id,
                DeletionDate = DateTime.Now,
                GuestId = app.LoggedUser.Id,
                OwnerId = accommodation.OwnerId
            };
            _ownerNotificationService.Save(notification);
        }


        
        public ICommand RateAccommodationCommand { get; private set; }
        
        private HashSet<int> _ratedReservations = new HashSet<int>();

        private void Rate(object parameter)
        {
            if (parameter is not AccommodationReservation selectedReservation)
                return;

            bool canRate = _accommodationReservationService.CanRateAccommodation(selectedReservation.Id);

            if (_ratedReservations.Contains(selectedReservation.Id))
            {
                MessageBox.Show("You have already rated this accommodation.");
                return;
            }

            if (!canRate)
            {
                MessageBox.Show("It is not possible to rate the accommodation because more than 5 days have passed since the end of the reservation.");
                return;
            }

            RateAccommodationViewModel rateViewModel = new RateAccommodationViewModel(selectedReservation, _guestWindow);
            RateAccommodationPage ratePage = new RateAccommodationPage();
            ratePage.DataContext = rateViewModel;

            rateViewModel.ReservationRated += (sender, reservationId) =>
            {
                _ratedReservations.Add(reservationId);
            };

            _guestWindow.MainFrameGuestWindow.Content = ratePage;
        }
        
        private void ViewRecensions(object parameter)
        {
            if (parameter is AccommodationReservation selectedReservation)
            {
                GuestReview guestReview = _guestReviewService.GetByReservationId(selectedReservation.Id);

                // Check if the guest review is available
                if (guestReview != null)
                {
                    bool canViewReview = _accommodationReservationService.CanGuestViewReview(selectedReservation.Id);

                    if (!canViewReview)
                    {
                        MessageBox.Show("You can only view your own review if you were the first to rate the owner and accommodation.");
                        return;
                    }

                    // Create the View Model for displaying the review
                    ViewRecensionViewModel reviewViewModel = new ViewRecensionViewModel(guestReview);                   

                    // Create the page for displaying the review
                    ViewRecensionPage reviewPage = new ViewRecensionPage();
                    reviewPage.DataContext = reviewViewModel;

                    // Set the main frame to the page for displaying the review
                    _guestWindow.MainFrameGuestWindow.Content = reviewPage;
                }
                else
                {
                    MessageBox.Show("Guest review not found.");
                }
            }
            else
            {
                MessageBox.Show("Invalid parameter type or parameter is null.");
            }
        }
        }
    }

