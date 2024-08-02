using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;
using BookingApp.WPF.View.GuestPages;

namespace BookingApp.WPF.ViewModel.GuestPages
{
    public class DetailsViewModel : ViewModelBase
    {
        private readonly IAccommodationReservationService _accommodationReservationService;
        private readonly IRenovationsService _renovationsService;
        private Accommodation _selectedAccommodation;
        private GuestWindow _guestWindow;
        private ISuperGuestService _superGuestService;
        public Accommodation SelectedAccommodation
        {
            get => _selectedAccommodation;
            set
            {
                _selectedAccommodation = value;
                OnPropertyChanged(nameof(SelectedAccommodation));
            }
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int GuestNumber { get; set; }
        public int NumberOfDays { get; set; }

        public ICommand SearchCommand { get; private set; }
        public ICommand ReserveCommand { get; private set; }
        public ICommand BackCommand { get; private set; }

        public ObservableCollection<AccommodationReservation> AvailableReservations { get; set; }
        public AccommodationReservation SelectedTermin { get; set; }

        public DetailsViewModel(Accommodation accommodation, GuestWindow guestWindow)
        {
            _accommodationReservationService = Injector.Container.Resolve<IAccommodationReservationService>();
            _superGuestService = Injector.Container.Resolve<ISuperGuestService>();
            _renovationsService = Injector.Container.Resolve<IRenovationsService>();
            StartDate = DateTime.Today; // Postavi na trenutni datum
            EndDate = DateTime.Today.AddDays(1);
            _guestWindow = guestWindow;
            SelectedAccommodation = accommodation;
            SearchCommand = new RelayCommand(ExecuteSearchCommand);
            ReserveCommand = new RelayCommand(ExecuteReserveCommand);
            BackCommand = new RelayCommand(ExecuteBackCommand);
            AvailableReservations = new ObservableCollection<AccommodationReservation>();
        }

        private void ExecuteSearchCommand(object parameter)
        {
            if (ValidateInput())
            {
                var accommodationReservation = new AccommodationReservation(StartDate, EndDate, GuestNumber, NumberOfDays, SelectedAccommodation.Id);
                AvailableReservations = new ObservableCollection<AccommodationReservation>(_renovationsService.FindAvailableReservations(SelectedAccommodation, accommodationReservation));
                OnPropertyChanged(nameof(AvailableReservations));
            }
        }

        private void ExecuteReserveCommand(object parameter)
        {
            if (SelectedTermin == null)
            {
                MessageBox.Show("Molimo vas da izaberete rezervaciju koju želite da sačuvate.");
                return;
            }
            
            var app = (App)Application.Current;
            _superGuestService.ConfirmReservation(
                SelectedTermin.GuestNumber,
                SelectedTermin.AccommodationId,
                app.LoggedUser.Id, // Assuming the logged-in user is the one making the reservation
                $"{SelectedTermin.StartDate:dd.MM.yyyy} - {SelectedTermin.EndDate:dd.MM.yyyy}",
                SelectedTermin.Duration// Assuming you have this property in your Accommodation model
            );

            _accommodationReservationService.Save(SelectedTermin);
            MessageBox.Show("Rezervacija uspešno sačuvana.");

            // Dodajte dodatne logike, kao što je ažuriranje prikaza ili navigacija na drugu stranicu
        }

        private void ExecuteBackCommand(object parameter)
        {
            // Implementacija logike za povratak na prethodnu stranicu
        }

        private bool ValidateInput()
        {
            if (StartDate.Date.CompareTo(EndDate.Date) > 0)
            {
                MessageBox.Show("Start date must be before end date.");
                return false;
            }

            if (StartDate.Date.CompareTo(DateTime.Today) < 0)
            {
                MessageBox.Show("Start date must be today or later.");
                return false;
            }

            if (GuestNumber <= 0)
            {
                MessageBox.Show("Please enter a valid guest number.");
                return false;
            }

            if (NumberOfDays <= 0)
            {
                MessageBox.Show("Please enter a valid number of days.");
                return false;
            }

            // Dodajte dodatne validacije po potrebi

            return true;
        }
    }
}
