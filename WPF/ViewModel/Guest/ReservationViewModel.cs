using System.Collections.Generic;
using System.Windows;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;

namespace BookingApp.WPF.ViewModel.GuestPages
{
    public class ReservationViewModel : ViewModelBase
    {
        private readonly GuestWindow _guestWindow;
        private readonly IAccommodationReservationService _accommodationReservationService;
        private readonly ISuperGuestService _superGuestService;

        public List<AccommodationReservation> AvailableReservations { get; set; }
        public AccommodationReservation SelectedTermin { get; set; }
        public Accommodation SelectedAccommodation { get; set; }

        public ReservationViewModel(List<AccommodationReservation> availableReservations, GuestWindow guestWindow)
        {
            AvailableReservations = availableReservations;
            _guestWindow = guestWindow;
            _superGuestService = Injector.Container.Resolve<ISuperGuestService>();
            _accommodationReservationService = Injector.Container.Resolve<IAccommodationReservationService>();
        }

        public void ReserveAccommodation()
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
            

           // _accommodationReservationService.Save(SelectedTermin);
            MessageBox.Show("Rezervacija uspešno sačuvana.");

            // Dodajte dodatne logike, kao što je ažuriranje prikaza ili navigacija na drugu stranicu
        }
    }
}