using System;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Dto;
using BookingApp.Repository;

namespace BookingApp.Service
{
    public class ReservationRescheduleService : IReservationRescheduleService
    {
        private readonly IReservationRescheduleRepository _reservationRescheduleRepository;
        private readonly IUserService _userService;
        private readonly IAccommodationService _accomodationService;
        private readonly IAccommodationReservationRepository _accomodationReservationRepository;

        public ReservationRescheduleService(IReservationRescheduleRepository reservationRescheduleRepository,
                                            IUserService userService, 
                                            IAccommodationService accommodationService, IAccommodationReservationRepository accommodationReservationRepository)
        {
            _reservationRescheduleRepository = reservationRescheduleRepository;
            _userService = userService;
            _accomodationService = accommodationService;
            _accomodationReservationRepository = accommodationReservationRepository;
        }

        public IEnumerable<ReservationRescheduleDto> GetAllByOwnerId(int ownerId)
        {
            return _reservationRescheduleRepository.GetAllByOwnerId(ownerId)
                .Select(e => new ReservationRescheduleDto
                {
                    Id = e.Id,
                    GuestId = e.GuestId,
                    ReservationId = e.ReservationId,
                    AccommodationId = e.AccommodationId,
                    Username = _userService.GetUsernameById(e.GuestId),
                    NameAccomodation = _accomodationService.GetAccommodationById(e.AccommodationId).Name,
                    NewStartDate = e.NewStartDate,
                    NewEndDate = e.NewEndDate,
                    Available = IsAvailable(e.NewStartDate, e.NewEndDate, e.AccommodationId, e.GuestId),
                    ReschedulingAnswerStatus = e.ReschedulingAnswerStatus,
                    RejectionComment = e.RejectionComment
                });
        }

        private string IsAvailable(DateTime newStartDate, DateTime newEndDate, int accommodationId, int guestId)
        {
            var reservations = _accomodationReservationRepository.GetAllReservationsByAccomodationId(accommodationId)
                .Where(e => e.GuestId != guestId).ToList();

            foreach (var reservation in reservations)
            {
                if (newStartDate >= reservation.StartDate && newEndDate <= reservation.EndDate)
                {
                    return "No";
                }
            }
            return "Yes";
        }

        /*public IEnumerable<AccommodationReservation> GetPendingReservations(int guestId)
        {
            var pendingReschedules = _reservationRescheduleRepository.GetAll()
                .Where(reschedule => reschedule.GuestId == guestId && reschedule.ReschedulingAnswerStatus == ReschedulingStatus.Pending)
                .ToList();

            var pendingReservationIds = pendingReschedules.Select(reschedule => reschedule.ReservationId);

            var pendingReservations = new List<AccommodationReservation>();

            foreach (var reschedule in pendingReschedules)
            {
                var pendingReservation = new AccommodationReservation
                {
                    Id = reschedule.ReservationId,
                    StartDate = reschedule.NewStartDate,
                    EndDate = reschedule.NewEndDate,
                };
                pendingReservations.Add(pendingReservation);
            }

            return pendingReservations;
        }*/



        public IEnumerable<ReservationReschedule> GetAcceptedReschedules(int guestId)
        {
            var allReschedules = _reservationRescheduleRepository.GetAll();
            return allReschedules.Where(reschedule => reschedule.GuestId == guestId && 
                                            reschedule.ReschedulingAnswerStatus == ReschedulingStatus.Accepted);
        }

        public IEnumerable<ReservationReschedule> GetRejectedReschedules(int guestId)
        {
            var allReschedules = _reservationRescheduleRepository.GetAll();
            return allReschedules.Where(reschedule => reschedule.GuestId == guestId && 
                                            reschedule.ReschedulingAnswerStatus == ReschedulingStatus.Rejected);
        }

        public IEnumerable<ReservationReschedule> GetPendingReschedules(int guestId)
        {
            var allReschedules = _reservationRescheduleRepository.GetAll();
            return allReschedules.Where(reschedule => reschedule.GuestId == guestId && 
                                            reschedule.ReschedulingAnswerStatus == ReschedulingStatus.Pending);
        }

        public ReservationReschedule GetById(int id)
        {
            return _reservationRescheduleRepository.GetById(id);
        }

        public object Save(ReservationReschedule reservationReschedule)
        {
            return _reservationRescheduleRepository.Save(reservationReschedule);
        }

        public object Update(ReservationReschedule reservationReschedule)
        {
            return _reservationRescheduleRepository.Update(reservationReschedule);
        }

        public IEnumerable<ReservationReschedule> GetAll(int ownerId)
        {
            return _reservationRescheduleRepository.GetAllByOwnerId(ownerId);
        }
        public IEnumerable<ReservationReschedule> GetAlll()
        {
            return _reservationRescheduleRepository.GetAll();
        }

        
        public ReservationReschedule GetByReservationId(int reservationId)
        {
            return _reservationRescheduleRepository.GetByReservationId(reservationId);
        }
    }
}
