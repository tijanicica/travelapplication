using System;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View.GuestPages;
using BookingApp.WPF.View.OwnerPages;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class ViewRecensionViewModel : ViewModelBase
    {
        public GuestReview GuestReview { get; }
        private IGuestReviewService _guestReviewService;
        private IAccommodationReservationService _accommodationReservationService;
        private IAccommodationService _accommodationService;
        private IOwnerService _ownerService;
        private IUserService _userService;


        public string AccommodationName => GetAccommodationName();
        public Location AccommodationLocation => GetAccommodationLocation();
        public int GuestNumber => GetGuestNumber();
        public DateTime Checkin => GetCheckin();
        public DateTime Checkout => GetCheckout();
        public string OwnerUsername => GetOwnerUsername();

        public int Cleanliness => GuestReview.Cleanliness;
        public int RuleFollowing => GuestReview.RuleFollowing;
        public string Comment => GuestReview.Comment;

        public ICommand BackCommand { get; private set; }

        public ViewRecensionViewModel(GuestReview guestReview)
        {
            GuestReview = guestReview;
            _guestReviewService = Injector.Container.Resolve<IGuestReviewService>();
            _accommodationReservationService = Injector.Container.Resolve<IAccommodationReservationService>();
            _accommodationService = Injector.Container.Resolve<IAccommodationService>();
            _ownerService = Injector.Container.Resolve<IOwnerService>();
            BackCommand = new DelegateCommand(ExecuteBackCommand);
        }
        
        private void ExecuteBackCommand(object obj)
        {
            // Prosleđujemo IAccommodationService
        }


        private string GetAccommodationName()
        {

            var reservation = _accommodationReservationService.GetById(GuestReview.ReservationId);
            var accommodationName = _accommodationService.GetAccommodationName(reservation.AccommodationId);
            return accommodationName;
        }

        private Location GetAccommodationLocation()
        { 
            var reservation = _accommodationReservationService.GetById(GuestReview.ReservationId);
            var accommodation = _accommodationService.GetAccommodationById(reservation.AccommodationId);
            Location location = accommodation.Location;

            return location;
        }

        private int GetGuestNumber()
        {
            var reservation = _accommodationReservationService.GetById(GuestReview.ReservationId);
            int guestNumber = reservation.GuestNumber;
            return guestNumber;
        }

        private DateTime GetCheckin()
        {
            var reservation = _accommodationReservationService.GetById(GuestReview.ReservationId);
            DateTime checkIn = reservation.StartDate;
            return checkIn;
        }

        private DateTime GetCheckout()
        {
            var reservation = _accommodationReservationService.GetById(GuestReview.ReservationId);
            DateTime checkOut = reservation.EndDate;
            return checkOut;
        }

        private string GetOwnerUsername()
        {
            var reservation = _accommodationReservationService.GetById(GuestReview.ReservationId);
            var accommodation = _accommodationService.GetAccommodationById(reservation.AccommodationId);
            var owner = _ownerService.GetById(accommodation.OwnerId);
            string ownerUsername = owner.Username;
            return ownerUsername;
        }
    }
}