using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;

namespace BookingApp.Repository;

public class TourReservationRepository : ITourReservationRepository
{
    private const string FilePath = "../../../Resources/Data/tourReservations.csv";

    private readonly Serializer<TourReservation> _serializer;

    private List<TourReservation> _tourReservations;
    
    public TourReservationRepository()
    {
        _serializer = new Serializer<TourReservation>();
        _tourReservations = _serializer.FromCSV(FilePath);
    }
    
    public TourReservation Save(TourReservation tourReservation)
    {
        tourReservation.Id = NextId();
        _tourReservations = _serializer.FromCSV(FilePath);
        _tourReservations.Add(tourReservation);
        _serializer.ToCSV(FilePath, _tourReservations);
        return tourReservation;
    }
    public IEnumerable<TourReservation> GetAll()
    {
        _tourReservations = _serializer.FromCSV(FilePath);
        return _tourReservations;
    }
    
    public int NextId()
    {
        _tourReservations = _serializer.FromCSV(FilePath);
        if (_tourReservations.Count < 1)
        {
            return 1;
        }
        return _tourReservations.Max(c => c.Id) + 1;
    }
    

    public TourReservation Update(TourReservation tourReservation)
    {
        _tourReservations = _serializer.FromCSV(FilePath);
        TourReservation current = _tourReservations.Find(c => c.Id == tourReservation.Id);
        int index = _tourReservations.IndexOf(current);
        _tourReservations.Remove(current);
        _tourReservations.Insert(index, tourReservation);       // keep ascending order of ids in file 
        _serializer.ToCSV(FilePath, _tourReservations);
        return tourReservation;
    }

    public TourReservation GetByTouristIdAndTourExecutionId(int touristId, int executionId)
    {
        return GetAll().Where(e => e.TouristId == touristId && e.TourExecutionId == executionId).FirstOrDefault();
    }
    
    
    
}