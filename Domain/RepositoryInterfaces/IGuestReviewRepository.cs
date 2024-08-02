using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.RepositoryInterfaces;

public interface IGuestReviewRepository
{ 
        GuestReview Save(GuestReview guestReview); 
        int NextId();
        List<GuestReview> GetAll();
}