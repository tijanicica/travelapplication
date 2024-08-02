using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.RepositoryInterfaces;

public interface IOwnerNotificationRepository
{
             OwnerNotification Save(OwnerNotification notification);

             int NextId();

             IEnumerable<OwnerNotification> GetAll();
             

             void Delete(OwnerNotification notification);

             OwnerNotification Update(OwnerNotification notification);

             OwnerNotification GetById(int id);
}