using System.Collections.Generic;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repository;

namespace BookingApp.Service;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    
    public NotificationService(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }
    
    public Notification Save(Notification notification)
    {
        return _notificationRepository.Save(notification);
    }
    
    public void Delete(Notification notification)
    {
        _notificationRepository.Delete(notification);
    }
    public Notification Update(Notification notification)
    {
        return _notificationRepository.Update(notification);
    }
    public Notification GetById(int id)
    {
        return _notificationRepository.GetById(id);
    }
    
    public IEnumerable<Notification> GetAll()
    {
        return _notificationRepository.GetAll();
    }

}