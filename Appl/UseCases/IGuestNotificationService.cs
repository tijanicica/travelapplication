

using System.Collections.Generic;
using System.Collections.ObjectModel;
using BookingApp.Domain.Model;

namespace BookingApp.Appl.UseCases;

public interface IGuestNotificationService
{
    GuestNotification Save(GuestNotification notification);
    public List<GuestNotification> GetAllByGuestId(int guestId);
    
}