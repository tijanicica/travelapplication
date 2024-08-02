using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Appl.UseCases;

public interface ITourGuideService
{
    void Delete(TourGuide user);
    TourGuide GetById(int id);
    TourGuide Update(TourGuide guide);
    List<TourGuide> GetAll();
}