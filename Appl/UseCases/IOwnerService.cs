using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Appl.UseCases;

public interface IOwnerService
{
    
    
    public void UpdateIsSuperOwner(int ownerId, bool isSuperOwner);
    public List<Owner> GetAll();
     Owner GetById(int id);

}