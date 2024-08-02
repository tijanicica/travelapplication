using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Appl.UseCases;

public interface IGuestReviewService
{
    GuestReview CreateReview(GuestReview guestReview);
    List<GuestReview> GetAll();
    GuestReview GetByReservationId(int id);
    public List<GuestReview> GetByOwnerId(int id);
}