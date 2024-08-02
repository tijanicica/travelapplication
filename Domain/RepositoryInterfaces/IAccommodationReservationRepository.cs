using System.Collections.Generic;
using System.Collections.ObjectModel;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.RepositoryInterfaces;

public interface IAccommodationReservationRepository
{ 
        ObservableCollection<AccommodationReservation> GetAllReservations();
        AccommodationReservation GetReservationById(int id);
        void Save(AccommodationReservation accommodationReservation);
        int NextId();
        List<AccommodationReservation> GetAllReservationsByAccomodationId(int accommodationId);
         void RemoveReservation(int reservationId);
         AccommodationReservation GetById(int reservationId);
          AccommodationReservation Update(AccommodationReservation accommodationReservation);
          public List<AccommodationReservation> GetByUser(User user);

}