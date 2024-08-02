using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.Service
{

    public class AccomodationReviewService: IAccommodationReviewService
    {
        private readonly IAccommodationReviewRepository _accommodationReviewRepository;
        private readonly IGuestReviewService _guestReviewService;
        private readonly IUserService _userService;
        //private readonly IAccommodationReservationService _accommodationReservationService;

        public AccomodationReviewService(IAccommodationReviewRepository accommodationReviewRepository,
            IUserService userService, IGuestReviewService guestReviewService)
        {
            _accommodationReviewRepository = accommodationReviewRepository;
            _userService = userService;
            _guestReviewService = guestReviewService;
           // _accommodationReservationService = accommodationReservationService;

        }
        public List<AccommodationReview> GetAll()
        {
            return _accommodationReviewRepository.GetAll().ToList();
        }
        public List<AccommodationReview> GetByOwnerId(int loggedUserId)
        {
            return _accommodationReviewRepository.GetAll().Where(e=> e.OwnerID == loggedUserId).ToList();
        }
        public List<AccommodationReview> GetByOwnerIdFiltered(int loggedUserId)
        {
            List<GuestReview> ownerGuestReviews = _guestReviewService.GetAll().Where(e => e.OwnerId == loggedUserId).ToList();

            List<AccommodationReview> accommodationReviews = GetByOwnerId(loggedUserId);

            List<AccommodationReview> filteredAccommodationReviews = new List<AccommodationReview>();

            foreach (var review in accommodationReviews)
            {
                if (ownerGuestReviews.Any(gr => gr.GuestId == review.GuestID))
                {
                    filteredAccommodationReviews.Add(review);
                }
            }

            return filteredAccommodationReviews;
        }


       public  AccommodationReview GetById(int ratingId)
        {
            return _accommodationReviewRepository.GetById(ratingId);
        }


        public void Save(AccommodationReview accommodationReview)
        {
            _accommodationReviewRepository.Save(accommodationReview);
        }
        
        /*public bool CanGuestViewReview(int reservationId)
        {
            // Logic to check if the guest can view their own review
            AccommodationReservation reservation = _accommodationReservationService.GetById(reservationId);
            

            if (reservation != null)
            {
                // Get all the guest reviews for the owner
                List<GuestReview> ownerGuestReviews = _guestReviewService.GetByOwnerId(reservation.Id);

                // Check if the guest was the first to rate the owner and accommodation
                return ownerGuestReviews.Any(gr => gr.GuestId == reservation.GuestId && gr.ReservationId < reservationId);
            }

            return false;
        }*/

    }

}