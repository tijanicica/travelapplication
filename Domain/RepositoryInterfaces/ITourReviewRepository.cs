using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.RepositoryInterfaces;

public interface ITourReviewRepository
{
     TourReview Save(TourReview tourReview);


     int NextId();


     IEnumerable<TourReview> GetAll();


     TourReview GetById(int id);



     TourReview Update(TourReview review);

}