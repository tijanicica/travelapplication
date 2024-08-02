using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.RepositoryInterfaces;

public interface IGuestRepository
{
        User GetByUsername(string username);
        Guest Save(Guest guest);
        int NextId();
        Guest GetById(int guestId);
        IEnumerable<Guest> GetAll(); 
        string GetNameById(int id);
        

}