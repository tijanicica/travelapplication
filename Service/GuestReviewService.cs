using System.Collections.Generic;
using System.Linq;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repository;

namespace BookingApp.Service;

public class GuestReviewService : IGuestReviewService
{
    private readonly IGuestReviewRepository _guestReviewRepository;
    
    public GuestReviewService(IGuestReviewRepository guestReviewRepository)
    {
        _guestReviewRepository = guestReviewRepository;
    }
    
    public GuestReview CreateReview(GuestReview guestReview)
    {
        return _guestReviewRepository.Save(guestReview);
    }

    public List<GuestReview> GetAll()
    {return  _guestReviewRepository.GetAll();
    }

    public GuestReview GetByReservationId(int id)
    {
        var guestReviews = _guestReviewRepository.GetAll();
        var guestReview = guestReviews.FirstOrDefault(gr => gr.ReservationId == id);
        return guestReview;
    }

    public List<GuestReview> GetByOwnerId(int id)
    {
        var guestReviews = _guestReviewRepository.GetAll();
        var reviews = guestReviews.Where(gr => gr.OwnerId == id).ToList();
        return reviews;
    }
}