using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Appl.UseCases;

public interface ITourReviewService
{


    TourReview Save(TourReview review);


    IEnumerable<TourReview> GetByTourGuideId(int tourGuideId);


    TourReview GetById(int id);


    TourReview Update(TourReview review);
    bool HasTourBeenRated(int touristId, int tourExecutionId);
    IEnumerable<TourReview> GetByTourExectuionId(int executionId);

}