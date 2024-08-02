using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Appl.UseCases;

public interface ITourReservationService
{

    IEnumerable<TourReservation> GetAll();
    TourReservation CreateReservation(TourReservation tourReservation);


    IEnumerable<TourReservation> GetByTourExecutionId(int id);


    TourReservation Update(TourReservation tourReservation);


    IEnumerable<PersonOnTour> GetPeopleOnTour(int touristId);


  //  Tour GetActiveTour(int touristId);


   // string GetCurrentTourSpot(int touristId);



    string TouristIsArrived(int touristId);


    List<PersonOnTour> GetArrivedPeopleOnTourByTouristId(int touristId, int executionId);


  //  List<TourDto> GetAllFinishedTours(int touristId);
   // List<TourDto> GetAllByTouristId(int touristId);
    

}