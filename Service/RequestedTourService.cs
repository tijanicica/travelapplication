using System.Collections.Generic;
using System.Linq;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repository;

namespace BookingApp.Service;

public class RequestedTourService : IRequestedTourService

{

    private readonly IRequestedTourRepository _requestedTourRepository;
    
    public RequestedTourService(IRequestedTourRepository requestedTourRepository)
    {
        _requestedTourRepository = requestedTourRepository;
    }


    public RequestedTour Save(RequestedTour requestedTour)
    {
        return _requestedTourRepository.Save(requestedTour);
    }

    

    
    public RequestedTour GetByTourRequestId(int tourRequestId)
    {
        return _requestedTourRepository.GetAll().Where(e=> e.TourRequestId == tourRequestId).FirstOrDefault();
    }

    public List<RequestedTour> GetAll()
    {
        return _requestedTourRepository.GetAll();
    }
}