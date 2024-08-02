using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Appl.UseCases;

public interface IRequestedTourService
{

    RequestedTour Save(RequestedTour requestedTour);

 


    RequestedTour GetByTourRequestId(int tourRequestId);
    List<RequestedTour> GetAll();

}