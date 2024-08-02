using System.Collections.Generic;
using System.Collections.Generic;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;

namespace BookingApp.Repository;

public class NotificationRepository : INotificationRepository
{
    private const string FilePath = "../../../Resources/Data/notifications.csv";

    private readonly Serializer<Notification> _serializer;

    private List<Notification> _notifications;

    public NotificationRepository()
    {
        _serializer = new Serializer<Notification>();
        _notifications = _serializer.FromCSV(FilePath);
    }
    
    public Notification Save(Notification notification)
    {
        notification.Id = NextId();
        _notifications = _serializer.FromCSV(FilePath);
        _notifications.Add(notification);
        _serializer.ToCSV(FilePath, _notifications);
        return notification;
    }
    
    public int NextId()
    {
        _notifications = _serializer.FromCSV(FilePath);
        if (_notifications.Count < 1)
        {
            return 1;
        }
        return _notifications.Max(c => c.Id) + 1;
    }
    
    public IEnumerable<Notification> GetAll()
    {
        UpdateList();
        return _notifications;
    }

    private void UpdateList()
    {
        _notifications = _serializer.FromCSV(FilePath);
    }
    
    public void Delete(Notification notification)
    {
        _notifications = _serializer.FromCSV(FilePath);
        Notification founded = _notifications.Find(c => c.Id == notification.Id);
        _notifications.Remove(founded);
        _serializer.ToCSV(FilePath, _notifications);
    }
    
    public Notification Update(Notification notification)
    {
        _notifications = _serializer.FromCSV(FilePath);
        Notification current = _notifications.Find(c => c.Id == notification.Id);
        int index = _notifications.IndexOf(current);
        _notifications.Remove(current);
        _notifications.Insert(index, notification);       // keep ascending order of ids in file 
        _serializer.ToCSV(FilePath, _notifications);
        return notification;
    }
    
    public Notification GetById(int id)
    {
        UpdateList();
        var notification = _notifications.FirstOrDefault(t => t.Id == id);
        return notification;
    }
}