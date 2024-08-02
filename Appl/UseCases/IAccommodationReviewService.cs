using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Appl.UseCases;

public interface IAccommodationReviewService
{
   List<AccommodationReview> GetByOwnerId(int loggedUserId);
   AccommodationReview GetById(int ownerId);
   List<AccommodationReview> GetByOwnerIdFiltered(int loggedUserId);

   void Save(AccommodationReview accommodationReview);
   public List<AccommodationReview> GetAll();
}