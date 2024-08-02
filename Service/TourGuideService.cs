using System.Collections.Generic;
using System.Linq;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repository;

namespace BookingApp.Service;

public class TourGuideService : ITourGuideService

{
    private readonly ITourGuideRepository _tourGuideRepository;
    
    public TourGuideService(ITourGuideRepository tourGuideRepository)
    {
        _tourGuideRepository = tourGuideRepository;
    }

    public void Delete(TourGuide user)
    {
        _tourGuideRepository.Delete(user);
    }

    public TourGuide GetById(int id)
    {
        return _tourGuideRepository.GetAll().Where(e => e.Id == id).FirstOrDefault();
    }

    public TourGuide Update(TourGuide guide)
    {
        return _tourGuideRepository.Update(guide);
    }

    public List<TourGuide> GetAll()
    {
        return _tourGuideRepository.GetAll();
    }

}