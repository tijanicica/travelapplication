using System.Collections.Generic;
using System.Linq;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repository;

namespace BookingApp.Service;

public class TourReviewService : ITourReviewService
{
    private readonly ITourReviewRepository _tourReviewRepository;
   
    
    

    public TourReviewService(ITourReviewRepository tourReviewRepository )
    {
        _tourReviewRepository = tourReviewRepository;

    }
    
    public TourReview Save(TourReview review)
    {
        return _tourReviewRepository.Save(review);
    }
    
    public IEnumerable<TourReview> GetByTourGuideId(int tourGuideId)
    {
        return _tourReviewRepository.GetAll().Where(e => e.TourGuideId == tourGuideId);
    }
    
    public TourReview GetById(int id)
    {
        return _tourReviewRepository.GetById(id);
    }
    
    public TourReview Update(TourReview review)
    {
        return _tourReviewRepository.Update(review);
    }
    
    public bool HasTourBeenRated(int touristId, int tourExecutionId)
    {
        return _tourReviewRepository.GetAll().Any(review =>
            review.TouristId == touristId && review.TourExecutionId == tourExecutionId);
    
    }

    public IEnumerable<TourReview> GetByTourExectuionId(int executionId)
    {
        return _tourReviewRepository.GetAll().Where(e => e.TourExecutionId == executionId);
    }
}