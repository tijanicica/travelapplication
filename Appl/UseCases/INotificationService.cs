using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Appl.UseCases;

public interface INotificationService
{
    Notification Save(Notification notification);


    void Delete(Notification notification);

    Notification Update(Notification notification);
    
     Notification GetById(int id);

     IEnumerable<Notification> GetAll();

}