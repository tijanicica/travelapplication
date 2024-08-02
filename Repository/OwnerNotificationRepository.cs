using BookingApp.Serializer;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.Repository
{
    public class OwnerNotificationRepository : IOwnerNotificationRepository
    {
        private const string FilePath = "../../../Resources/Data/ownernotifications.csv";

        private readonly Serializer<OwnerNotification> _serializer;

        private List<OwnerNotification> _ownerNotifications;

        public OwnerNotificationRepository()
        {
            _serializer = new Serializer<OwnerNotification>();
            _ownerNotifications = _serializer.FromCSV(FilePath);
        }

        public OwnerNotification Save(OwnerNotification notification)
        {
            notification.Id = NextId();
            _ownerNotifications.Add(notification);
            _serializer.ToCSV(FilePath, _ownerNotifications);
            return notification;
        }

        public int NextId()
        {
            if (_ownerNotifications.Count < 1)
            {
                return 1;
            }
            return _ownerNotifications.Max(c => c.Id) + 1;
        }

        public IEnumerable<OwnerNotification> GetAll()
        {
            UpdateList();
            return _ownerNotifications;
        }

        private void UpdateList()
        {
            _ownerNotifications = _serializer.FromCSV(FilePath);
        }

        public void Delete(OwnerNotification notification)
        {
            _ownerNotifications.Remove(notification);
            _serializer.ToCSV(FilePath, _ownerNotifications);
        }

        public OwnerNotification Update(OwnerNotification notification)
        {
            var current = _ownerNotifications.FirstOrDefault(c => c.Id == notification.Id);
            if (current != null)
            {
                int index = _ownerNotifications.IndexOf(current);
                _ownerNotifications[index] = notification; // keep ascending order of ids in file 
                _serializer.ToCSV(FilePath, _ownerNotifications);
            }
            return notification;
        }

        public OwnerNotification GetById(int id)
        {
            UpdateList();
            var notification = _ownerNotifications.FirstOrDefault(t => t.Id == id);
            return notification;
        }
    }
}
