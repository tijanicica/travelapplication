using System.Collections.Generic;
using System.Linq;
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

public class RequestedTourRepository : IRequestedTourRepository
{
      private const string FilePath = "../../../Resources/Data/requestedTours.csv";
    
            private readonly Serializer<RequestedTour> _serializer;
    
            private List<RequestedTour> _requestedTours;
    
            public RequestedTourRepository()
            {
                _serializer = new Serializer<RequestedTour>();
                _requestedTours = _serializer.FromCSV(FilePath);
            }
    
            public List<RequestedTour> GetAll()
            {
                return _serializer.FromCSV(FilePath);
            }
    
            public RequestedTour Save(RequestedTour requestedTour)
            {
                requestedTour.Id = NextId();
                _requestedTours = _serializer.FromCSV(FilePath);
                _requestedTours.Add(requestedTour);
                _serializer.ToCSV(FilePath, _requestedTours);
                return requestedTour;
            }
    
            public int NextId()
            {
                _requestedTours = _serializer.FromCSV(FilePath);
                if (_requestedTours.Count < 1)
                {
                    return 1;
                }
                return _requestedTours.Max(c => c.Id) + 1;
            }
    
            public void Delete(RequestedTour requestedTour)
            {
                _requestedTours = _serializer.FromCSV(FilePath);
                RequestedTour founded = _requestedTours.Find(c => c.Id == requestedTour.Id);
                _requestedTours.Remove(founded);
                _serializer.ToCSV(FilePath, _requestedTours);
            }
    
            public RequestedTour Update(RequestedTour requestedTour)
            {
                _requestedTours = _serializer.FromCSV(FilePath);
                RequestedTour current = _requestedTours.Find(c => c.Id == requestedTour.Id);
                int index = _requestedTours.IndexOf(current);
                _requestedTours.Remove(current);
                _requestedTours.Insert(index, requestedTour);       // keep ascending order of ids in file 
                _serializer.ToCSV(FilePath, _requestedTours);
                return requestedTour;
            }
}