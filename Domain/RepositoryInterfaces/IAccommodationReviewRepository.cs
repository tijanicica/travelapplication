using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.RepositoryInterfaces;

public interface IAccommodationReviewRepository
{
    List<AccommodationReview> GetAll();
    AccommodationReview GetById(int ratingId);

    AccommodationReview Save(AccommodationReview review);
    int NextId();
}