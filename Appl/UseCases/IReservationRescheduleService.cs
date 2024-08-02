using System.Collections.Generic;
using BookingApp.Domain.Model;
using BookingApp.Dto;

namespace BookingApp.Appl.UseCases;

public interface IReservationRescheduleService
{
    IEnumerable<ReservationReschedule> GetAll(int ownerId);

    public IEnumerable<ReservationReschedule> GetAcceptedReschedules(int guestId);

    public IEnumerable<ReservationReschedule> GetRejectedReschedules(int guestId);

    public IEnumerable<ReservationReschedule> GetPendingReschedules(int guestId);
    
    //void Save(ReservationReschedule reservationReschedule);
    //IEnumerable<ReservationRescheduleDto> GetAll(int ownerId);
    IEnumerable<ReservationRescheduleDto> GetAllByOwnerId(int ownerId);

     ReservationReschedule GetById(int id);


     object Save(ReservationReschedule reservationReschedule);


      object Update(ReservationReschedule reservationReschedule);
      //public IEnumerable<AccommodationReservation> GetPendingReservations(int guestId);

      public ReservationReschedule GetByReservationId(int reservationId);
      public IEnumerable<ReservationReschedule> GetAlll();


}