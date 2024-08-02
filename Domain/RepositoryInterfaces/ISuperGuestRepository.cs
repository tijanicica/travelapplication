using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.RepositoryInterfaces;

public interface ISuperGuestRepository
{
    public List<SuperGuest> GetAll();

    public SuperGuest Save(SuperGuest superGuest);

    public int NextId();

    public void Delete(SuperGuest superGuest);

    public SuperGuest Update(SuperGuest superGuest);

    public SuperGuest GetById(int id);

    public SuperGuest GetByGuestId(int guestId);
}