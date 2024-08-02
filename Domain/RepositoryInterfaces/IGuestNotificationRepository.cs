using System.Collections.Generic;
using BookingApp.Domain.Model;


namespace BookingApp.Domain.RepositoryInterfaces;

public interface IGuestNotificationRepository
{
    GuestNotification Save(GuestNotification notification);

    int NextId();

    IEnumerable<GuestNotification> GetAll();
             

    void Delete(GuestNotification notification);

    GuestNotification Update(GuestNotification notification);

    GuestNotification GetById(int id);
}