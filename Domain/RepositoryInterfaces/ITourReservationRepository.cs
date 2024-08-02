using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.RepositoryInterfaces;

public interface ITourReservationRepository
{
    TourReservation Save(TourReservation tourReservation);

    IEnumerable<TourReservation> GetAll();


    int NextId();



    TourReservation Update(TourReservation tourReservation);


    TourReservation GetByTouristIdAndTourExecutionId(int touristId, int executionId);

}