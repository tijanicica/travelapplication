using System.Collections.Generic;
using System.Collections.Generic;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;

namespace BookingApp.Repository;

public class TourReviewRepository : ITourReviewRepository
{
    private const string FilePath = "../../../Resources/Data/tourReviews.csv";

    private readonly Serializer<TourReview> _serializer;

    private List<TourReview> _tourReviews;

    public TourReviewRepository()
    {
        _serializer = new Serializer<TourReview>();
        _tourReviews = _serializer.FromCSV(FilePath);
    }
    
    public TourReview Save(TourReview tourReview)
    {
        tourReview.Id = NextId();
        _tourReviews = _serializer.FromCSV(FilePath);
        _tourReviews.Add(tourReview);
        _serializer.ToCSV(FilePath, _tourReviews);
        return tourReview;
    }
    
    public int NextId()
    {
        _tourReviews = _serializer.FromCSV(FilePath);
        if (_tourReviews.Count < 1)
        {
            return 1;
        }
        return _tourReviews.Max(c => c.Id) + 1;
    }
    
    public IEnumerable<TourReview> GetAll()
    {
        _tourReviews = _serializer.FromCSV(FilePath);
        return _tourReviews;
    }
    
    public TourReview GetById(int id)
    {
        var review = _tourReviews.FirstOrDefault(u => u.Id == id);
        return review;
    }
    
    
    public TourReview Update(TourReview review)
    {
        _tourReviews = _serializer.FromCSV(FilePath);
        TourReview current = _tourReviews.Find(c => c.Id == review.Id);
        int index = _tourReviews.IndexOf(current);
        _tourReviews.Remove(current);
        _tourReviews.Insert(index, review);     
        _serializer.ToCSV(FilePath, _tourReviews);
        return review;
    }
}