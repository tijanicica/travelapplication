using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Appl.UseCases;

public interface IOwnerNotificationService
{
    public OwnerNotification Save(OwnerNotification notification);
   // public List<AccommodationReservation> ChechkRateAGuestNotifications(User loggedUser);

    public bool isReservationForLoggedOwnersAccommodation(AccommodationReservation ar, User loggedUser);
    public bool IsFiveDaysPassed(AccommodationReservation ar);

}