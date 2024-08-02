using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;

using BookingApp.Serializer;
namespace BookingApp.Repository;

public class GuestNotificationRepository: IGuestNotificationRepository

{
    private const string FilePath = "../../../Resources/Data/guestnotifications.csv";

        private readonly Serializer<GuestNotification> _serializer;

        private List<GuestNotification> _guestNotifications;

        public GuestNotificationRepository()
        {
            _serializer = new Serializer<GuestNotification>();
            _guestNotifications = _serializer.FromCSV(FilePath);
        }

        public GuestNotification Save(GuestNotification notification)
        {
            notification.Id = NextId();
            _guestNotifications.Add(notification);
            _serializer.ToCSV(FilePath, _guestNotifications);
            return notification;
        }

        public int NextId()
        {
            if (_guestNotifications.Count < 1)
            {
                return 1;
            }
            return _guestNotifications.Max(c => c.Id) + 1;
        }

        public IEnumerable<GuestNotification> GetAll()
        {
            UpdateList();
            return _guestNotifications;
        }

        private void UpdateList()
        {
            _guestNotifications = _serializer.FromCSV(FilePath);
        }

        public void Delete(GuestNotification notification)
        {
            _guestNotifications.Remove(notification);
            _serializer.ToCSV(FilePath, _guestNotifications);
        }

        public GuestNotification Update(GuestNotification notification)
        {
            var current = _guestNotifications.FirstOrDefault(c => c.Id == notification.Id);
            if (current != null)
            {
                int index = _guestNotifications.IndexOf(current);
                _guestNotifications[index] = notification; // keep ascending order of ids in file 
                _serializer.ToCSV(FilePath, _guestNotifications);
            }
            return notification;
        }

        public GuestNotification GetById(int id)
        {
            UpdateList();
            var notification = _guestNotifications.FirstOrDefault(t => t.Id == id);
            return notification;
        }
        
}