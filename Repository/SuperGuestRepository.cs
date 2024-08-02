using BookingApp.Serializer;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.Repository;

public class SuperGuestRepository : ISuperGuestRepository
{
    
    private const string FilePath = "../../../Resources/Data/superguests.csv";
    private readonly Serializer<SuperGuest> _serializer;

    private List<SuperGuest> _superGuests;

    public SuperGuestRepository()
    {
        _serializer = new Serializer<SuperGuest>();
        _superGuests = _serializer.FromCSV(FilePath);
    }

    public List<SuperGuest> GetAll()
    {
        return _serializer.FromCSV(FilePath);
    }

    public SuperGuest Save(SuperGuest superGuest)
    {
        superGuest.Id = NextId();
        _superGuests = _serializer.FromCSV(FilePath);
        _superGuests.Add(superGuest);
        _serializer.ToCSV(FilePath, _superGuests);
        return superGuest;
    }

    public int NextId()
    {
        _superGuests = _serializer.FromCSV(FilePath);
        if (_superGuests.Count < 1)
        {
            return 1;
        }
        return _superGuests.Max(g => g.Id) + 1;
    }

    public void Delete(SuperGuest superGuest)
    {
        _superGuests = _serializer.FromCSV(FilePath);
        SuperGuest founded = _superGuests.Find(g => g.Id == superGuest.Id);
        _superGuests.Remove(founded);
        _serializer.ToCSV(FilePath, _superGuests);
    }

    public SuperGuest Update(SuperGuest superGuest)
    {
        _superGuests = _serializer.FromCSV(FilePath);
        SuperGuest current = _superGuests.Find(g => g.Id == superGuest.Id);
        int index = _superGuests.IndexOf(current);
        _superGuests.Remove(current);
        _superGuests.Insert(index, superGuest);
        _serializer.ToCSV(FilePath, _superGuests);
        return superGuest;
    }

    public SuperGuest GetById(int id)
    {
        _superGuests = _serializer.FromCSV(FilePath);
        return _superGuests.Find(g => g.Id == id);
    }

    public SuperGuest GetByGuestId(int guestId)
    {
        _superGuests = _serializer.FromCSV(FilePath);
        return _superGuests.Find(g => g.GuestId == guestId);
    }
    
}