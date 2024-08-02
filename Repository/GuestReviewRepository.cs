using System.Collections.Generic;
using System.IO;
using System.Linq;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;
namespace BookingApp.Repository;

public class GuestReviewRepository : IGuestReviewRepository
{
    private const string FilePath = "../../../Resources/Data/guestReview.csv";

    private readonly Serializer<GuestReview> _serializer;

    private List<GuestReview> _guestReview;
    
    public GuestReviewRepository()
    {
        _serializer = new Serializer<GuestReview>();
        _guestReview = _serializer.FromCSV(FilePath);
    }
    
    public GuestReview Save(GuestReview guestReview)
    {
        guestReview.Id = NextId();
        _guestReview = _serializer.FromCSV(FilePath);
        _guestReview.Add(guestReview);
        _serializer.ToCSV(FilePath, _guestReview);
        return guestReview;
    }
    public List<GuestReview> GetAll()
    {
        return _serializer.FromCSV(FilePath);
    }
    public int NextId()
    {
        _guestReview = _serializer.FromCSV(FilePath);
        if (_guestReview.Count < 1)
        {
            return 1;
        }
        return _guestReview.Max(c => c.Id) + 1;
    }
}