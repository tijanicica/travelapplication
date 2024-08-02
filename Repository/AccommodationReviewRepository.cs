using System.Collections.Generic;
using System.Linq;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;

namespace BookingApp.Repository;

public class AccommodationReviewRepository : IAccommodationReviewRepository
{
    private const string FilePath = "../../../Resources/Data/accommodationReview.csv";

    private readonly Serializer<AccommodationReview> _serializer;
    private List<AccommodationReview> _accommodationReviews;

    public AccommodationReviewRepository()
    {
        _serializer = new Serializer<AccommodationReview>(); // Ispravka ovde
        _accommodationReviews = _serializer.FromCSV(FilePath);
    }
  
   
    public List<AccommodationReview> GetAll()
    {
        UpdateList();
        return _accommodationReviews;
    }

    public AccommodationReview GetById(int ratingId)
    {
        UpdateList();
        var execution = _accommodationReviews.FirstOrDefault(t => t.RatingID == ratingId);
        return execution;
    }
    private void UpdateList()
    {
        _accommodationReviews = _serializer.FromCSV(FilePath);
    }

    public AccommodationReview Save(AccommodationReview review)
    {
        review.RatingID = NextId();
        _accommodationReviews = _serializer.FromCSV(FilePath);
        _accommodationReviews.Add(review);
        _serializer.ToCSV(FilePath, _accommodationReviews);
        return review;

    }
    public int NextId()
    {
        _accommodationReviews = _serializer.FromCSV(FilePath);
        if (_accommodationReviews.Count < 1)
        {
            return 1;
        }
        return _accommodationReviews.Max(c => c.RatingID) + 1;
    }
    
   


}