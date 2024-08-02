using System.Collections.Generic;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Generic;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;


namespace BookingApp.Repository;

public class TourRequestRepository : ITourRequestRepository
{
     private const string FilePath = "../../../Resources/Data/tourRequests.csv";
    
            private readonly Serializer<TourRequest> _serializer;
    
            private List<TourRequest> _tourRequests;
    
            public TourRequestRepository()
            {
                _serializer = new Serializer<TourRequest>();
                _tourRequests = _serializer.FromCSV(FilePath);
            }
    
            public List<TourRequest> GetAll()
            {
                return _serializer.FromCSV(FilePath);
            }
    
            public TourRequest Save(TourRequest tourRequest)
            {
                tourRequest.Id = NextId();
                _tourRequests = _serializer.FromCSV(FilePath);
                _tourRequests.Add(tourRequest);
                _serializer.ToCSV(FilePath, _tourRequests);
                return tourRequest;
            }
    
            public int NextId()
            {
                _tourRequests = _serializer.FromCSV(FilePath);
                if (_tourRequests.Count < 1)
                {
                    return 1;
                }
                return _tourRequests.Max(c => c.Id) + 1;
            }
    
            public void Delete(TourRequest tourRequest)
            {
                _tourRequests = _serializer.FromCSV(FilePath);
                TourRequest founded = _tourRequests.Find(c => c.Id == tourRequest.Id);
                _tourRequests.Remove(founded);
                _serializer.ToCSV(FilePath, _tourRequests);
            }
    
            public TourRequest Update(TourRequest tourRequest)
            {
                _tourRequests = _serializer.FromCSV(FilePath);
                TourRequest current = _tourRequests.Find(c => c.Id == tourRequest.Id);
                int index = _tourRequests.IndexOf(current);
                _tourRequests.Remove(current);
                _tourRequests.Insert(index, tourRequest);       // keep ascending order of ids in file 
                _serializer.ToCSV(FilePath, _tourRequests);
                return tourRequest;
            }
}