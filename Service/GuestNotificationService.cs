
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.Service;

public class GuestNotificationService:IGuestNotificationService
{
    private readonly IGuestNotificationRepository _guestNotificationRepository;
    
    public GuestNotificationService(IGuestNotificationRepository guestNotificationRepository)
    {
        _guestNotificationRepository = guestNotificationRepository;
    }

    public GuestNotification Save(GuestNotification notification)
    {
        return _guestNotificationRepository.Save(notification);
    }

    public List<GuestNotification> GetAllByGuestId(int guestId)
    {
       List<GuestNotification> allNotifications = _guestNotificationRepository.GetAll().ToList();
       var app = Application.Current as App;
       List<GuestNotification> guestNotifications = allNotifications.Where(e => e.GuestId ==  app.LoggedUser.Id).ToList();
       return guestNotifications;
    }
}