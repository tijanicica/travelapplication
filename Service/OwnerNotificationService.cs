using System;
using System.Collections.Generic;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repository;

namespace BookingApp.Service;

public class OwnerNotificationService : IOwnerNotificationService
{
    private readonly IOwnerNotificationRepository _ownerNotificationRepository;
    private readonly IAccommodationService _accommodationService;
   // private readonly IAccommodationReservationService _reservationService;
    
    public OwnerNotificationService(IOwnerNotificationRepository ownerNotificationRepository,
        IAccommodationService accommodationService)
    {
        _ownerNotificationRepository = ownerNotificationRepository;
        _accommodationService = accommodationService;
       // _reservationService = reservationService;
    }

    public OwnerNotification Save(OwnerNotification notification)
    {
       return _ownerNotificationRepository.Save(notification);
    }
    
    /*  public List<AccommodationReservation> ChechkRateAGuestNotifications(User loggedUser)
    {
        List<AccommodationReservation> unratedReservations = new List<AccommodationReservation>();
        foreach (AccommodationReservation ar in _reservationService.GetAllReservations())
        {
            if (isReservationForLoggedOwnersAccommodation(ar, loggedUser))
            {
                if (!ar.isGuestRated && !IsFiveDaysPassed(ar))
                {
                    ar.reservedAccommodation = _accommodationService.GetAccommodationById(ar.Id);
                    unratedReservations.Add(ar);
                    /*rateTheGuest = new RateTheGuest(ar, loggedUser);
                    rateTheGuest.ShowDialog();
                }
            }
        }
        return unratedReservations;
    }*/
        
    public bool isReservationForLoggedOwnersAccommodation(AccommodationReservation ar, User loggedUser)
    {
        foreach (Accommodation a in _accommodationService.GetAllAccommodations())
        {
            if (a.Id == loggedUser.Id && ar.Id == a.Id)
            {
                return true;
            }
        }
        return false;
    }
    public bool IsFiveDaysPassed(AccommodationReservation ar)
    {
        DateTime currentDateTime = DateTime.Now;
        TimeSpan timeDifference;

        if (currentDateTime > ar.EndDate)
        {
            timeDifference = currentDateTime - ar.EndDate;

            return timeDifference.Days > 5;

        }
        else
        {
            return true;
        }
    }
}