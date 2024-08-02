using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repository;
using BookingApp.Utils;

namespace BookingApp.WPF.View.GuestPages
{
    public partial class ReservationPage : Page
    {
        public List<AccommodationReservation> AvailableReservations { get; set; }
        public AccommodationReservation SelectedReservation { get; set; }
        private IAccommodationReservationService _accommodationReservationService;
        private ISuperGuestService _superGuestService;

        public Accommodation SelectedAccommodation { get; set; }

        public ReservationPage(List<AccommodationReservation> availableReservations)
        {
            InitializeComponent();
            _accommodationReservationService = Injector.Container.Resolve<IAccommodationReservationService>();
            _superGuestService = Injector.Container.Resolve<ISuperGuestService>();
            AvailableReservations = availableReservations;
            DataContext = this;
            ShowReservations();
        }

        public ReservationPage(Accommodation selectedAccommodation, List<AccommodationReservation> availableReservations)
        {
            InitializeComponent();
            _accommodationReservationService = Injector.Container.Resolve<IAccommodationReservationService>();
            _superGuestService = Injector.Container.Resolve<ISuperGuestService>();
            SelectedAccommodation = selectedAccommodation;
            AvailableReservations = availableReservations;
            DataContext = this;
            ShowReservations();
        }

        private void ShowReservations()
        {
            ReservationListBox.ItemsSource = AvailableReservations;
        }

        private void Reserve_Click(object sender, RoutedEventArgs e)
        {
            if (ReservationListBox.SelectedItem == null)
            {
                MessageBox.Show("Molimo vas da izaberete rezervaciju koju želite da sačuvate.");
                return;
            }

            SelectedReservation = (AccommodationReservation)ReservationListBox.SelectedItem;
            var app = (App)Application.Current;

            _superGuestService.ConfirmReservation(
                SelectedReservation.GuestNumber,
                SelectedReservation.AccommodationId,
                app.LoggedUser.Id, // Assuming the logged-in user is the one making the reservation
                $"{SelectedReservation.StartDate:dd.MM.yyyy} - {SelectedReservation.EndDate:dd.MM.yyyy}",
                SelectedReservation.Duration// Assuming you have this property in your Accommodation model
            );

            MessageBox.Show("Rezervacija uspešno sačuvana.");
        }
    }
}
