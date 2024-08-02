using System.Collections.Generic;
using System.Linq;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;
namespace BookingApp.Repository;
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

public class TourRepository : ITourRepository
{

       private const string FilePath = "../../../Resources/Data/tours.csv";
    
            private readonly Serializer<Tour> _serializer;
    
            private List<Tour> _tours;
    
            public TourRepository()
            {
                _serializer = new Serializer<Tour>();
                _tours = _serializer.FromCSV(FilePath);
            }
    
            public string GetNameById(int id)
            {
                UpdateList();
                return GetById(id).Name;
                
            }
            public List<string> GetPhotosById(int id)
            {
                UpdateList();
                return GetById(id).Photos;
                
            }
            
            public string GetCountryByTourId(int id)
            {
                UpdateList();
                return GetById(id).Location.Country;
                
            }
            
            public string GetCityByTourId(int id)
            {
                UpdateList();
                return GetById(id).Location.City;
                
            }
            
            public double GetDurationByTourId(int id)
            {
                UpdateList();
                return GetById(id).Duration;
                
            }
            
            public int GetMaxTouristNumberByTourId(int id)
            {
                UpdateList();
                return GetById(id).MaxTouristNumber;
                
            }
            
            public Language GetLanguageByTourId(int id)
            {
                UpdateList();
                return GetById(id).Language;
                
            }
            
    
            public Tour Save(Tour tour)
            {
                tour.Id = NextId();
                _tours = _serializer.FromCSV(FilePath);
                _tours.Add(tour);
                _serializer.ToCSV(FilePath, _tours);
                return tour;
            }
 
            public int NextId()
            {
                _tours = _serializer.FromCSV(FilePath);
                if (_tours.Count < 1)
                {
                    return 1;
                }
                return _tours.Max(c => c.Id) + 1;
            }
            public int GetTourGuideIdByTourId(int tourId)
            {
                var tour = GetById(tourId);
                if (tour != null)
                {
                    return tour.TourGuideId;
                }

                return -1;
            }

            
        public IEnumerable<Tour> GetAll()
        {
            UpdateList();
            return _tours;
        }

        private void UpdateList()
        {
            _tours = _serializer.FromCSV(FilePath);
        }
        public Tour GetById(int Id)
        {
            UpdateList();
            var tour = _tours.FirstOrDefault(t => t.Id == Id);
            return tour;
        }
        
        public Tour Update(Tour tour)
        {
            _tours = _serializer.FromCSV(FilePath);
            Tour current = _tours.Find(c => c.Id == tour.Id);
            int index = _tours.IndexOf(current);
            _tours.Remove(current);
            _tours.Insert(index, tour);     
            _serializer.ToCSV(FilePath, _tours);
            return tour;
        }
        
        public void Delete(Tour tour)
        {
            _tours = _serializer.FromCSV(FilePath);
            Tour founded = _tours.Find(c => c.Id == tour.Id);
            _tours.Remove(founded);
            _serializer.ToCSV(FilePath, _tours);
        }
        
    
}

