using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.RepositoryInterfaces;

public interface IReservationRescheduleRepository
{
    IEnumerable<ReservationReschedule> GetAll();
    //public void Save(ReservationReschedule reservationReschedule);
    public int NextId();
   // IEnumerable<ReservationReschedule> GetAll();
    ReservationReschedule GetById(int id);

    object Save(ReservationReschedule reservationReschedule);

    object Update(ReservationReschedule reservationReschedule);
    IEnumerable<ReservationReschedule> GetAllByOwnerId();
    IEnumerable<ReservationReschedule> GetAllByOwnerId(int ownerId);
    ReservationReschedule GetByReservationId(int reservationId);
    


}