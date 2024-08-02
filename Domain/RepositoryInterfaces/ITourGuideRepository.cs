using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.RepositoryInterfaces;

public interface ITourGuideRepository
{
     User GetByUsername(string username);
     void Delete(TourGuide user);
     List<TourGuide> GetAll();

     TourGuide Update(TourGuide guide);

}
