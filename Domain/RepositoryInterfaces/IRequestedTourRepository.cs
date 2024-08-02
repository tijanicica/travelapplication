using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.RepositoryInterfaces;

public interface IRequestedTourRepository
{
    List<RequestedTour> GetAll();
    RequestedTour Save(RequestedTour requestedTour);
    int NextId();
    void Delete(RequestedTour requestedTour);
    RequestedTour Update(RequestedTour requestedTour);
}
