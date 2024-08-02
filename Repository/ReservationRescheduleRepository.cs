using BookingApp.Serializer;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Dto;
using BookingApp.Serializer;

namespace BookingApp.Repository;

public class ReservationRescheduleRepository : IReservationRescheduleRepository
{
    private const string FilePath = "../../../Resources/Data/reservationReschedule.csv";

    private readonly Serializer<ReservationReschedule> _serializer;
    private List<ReservationReschedule> _reservationReschedules;

    public ReservationRescheduleRepository()
    {
        _serializer = new Serializer<ReservationReschedule>();
        _reservationReschedules = _serializer.FromCSV(FilePath);
    }
    public IEnumerable<ReservationReschedule> GetAll()
    {
        UpdateList();
        return _reservationReschedules;  
    }
    public IEnumerable<ReservationReschedule> GetAllByOwnerId(int ownerId)
    {
        UpdateList();
        return _reservationReschedules.Where(r => r.OwnerId == ownerId);  
    }

    public IEnumerable<ReservationReschedule> GetAllByOwnerId()
    {
        UpdateList();
        return  _reservationReschedules;  
    }
    private void UpdateList()
    {
        _reservationReschedules = _serializer.FromCSV(FilePath);
    }
    


    public ReservationReschedule GetById(int id)
    {
        UpdateList();
        var reschedule = _reservationReschedules.FirstOrDefault(t => t.Id == id);
        return reschedule;
    }

    public object Save(ReservationReschedule reservationReschedule)
    {
        reservationReschedule.Id = NextId();
        _reservationReschedules = _serializer.FromCSV(FilePath);
        _reservationReschedules.Add(reservationReschedule);
        _serializer.ToCSV(FilePath, _reservationReschedules);
        return reservationReschedule;
    }
    public int NextId()
    {
        _reservationReschedules = _serializer.FromCSV(FilePath);
        if (_reservationReschedules.Count < 1)
        {
            return 1;
        }

        return _reservationReschedules.Max(c => c.Id) + 1;
    }



    public object Update(ReservationReschedule reservationReschedule)
    {
        _reservationReschedules = _serializer.FromCSV(FilePath);
        ReservationReschedule current = _reservationReschedules.Find(c => c.Id == reservationReschedule.Id);
        int index = _reservationReschedules.IndexOf(current);
        _reservationReschedules.Remove(current);
        _reservationReschedules.Insert(index, reservationReschedule);       // keep ascending order of ids in file 
        _serializer.ToCSV(FilePath, _reservationReschedules);
        return reservationReschedule;
    }
    
    public ReservationReschedule GetByReservationId(int reservationId)
    {
        UpdateList();
        return _reservationReschedules.FirstOrDefault(r => r.ReservationId == reservationId);
    }
}