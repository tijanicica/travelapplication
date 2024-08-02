using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.RepositoryInterfaces;

public interface INotificationRepository
{
    Notification Save(Notification notification);


    int NextId();


    IEnumerable<Notification> GetAll();




    void Delete(Notification notification);


    Notification Update(Notification notification);


    Notification GetById(int id);

}