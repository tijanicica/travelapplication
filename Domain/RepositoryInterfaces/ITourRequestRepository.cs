using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.RepositoryInterfaces;

public interface ITourRequestRepository
{
    List<TourRequest> GetAll();
    TourRequest Save(TourRequest tourRequest);
    int NextId();
    void Delete(TourRequest tourRequest);
    TourRequest Update(TourRequest tourRequest);
}