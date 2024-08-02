using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.RepositoryInterfaces;

public interface IOwnerRepository
{ 
        User GetByUsername(string username);
         void UpdateIsSuperOwner(int ownerId, bool isSuperOwner);
         List<Owner> GetAll();
         public Owner GetById(int id);
}