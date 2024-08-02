using BookingApp.Serializer;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.Repository
{
    public class TourGuideRepository : ITourGuideRepository
    {
        private const string FilePath = "../../../Resources/Data/tourGuides.csv";

        private readonly Serializer<TourGuide> _serializer;

        private List<TourGuide> _tourGuides;

        public TourGuideRepository()
        {
            _serializer = new Serializer<TourGuide>();
            _tourGuides = _serializer.FromCSV(FilePath);
        }

        public User GetByUsername(string username)
        {
            _tourGuides = _serializer.FromCSV(FilePath);
            return _tourGuides.FirstOrDefault(u => u.Username == username);
        }

        
        
        public void Delete(TourGuide user)
        {
            _tourGuides = _serializer.FromCSV(FilePath);
            TourGuide founded = _tourGuides.Find(c => c.Id == user.Id);
            _tourGuides.Remove(founded);
            _serializer.ToCSV(FilePath, _tourGuides);
        }
        
        public List<TourGuide> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        
        public TourGuide Update(TourGuide guide)
        {
            _tourGuides = _serializer.FromCSV(FilePath);
            TourGuide current = _tourGuides.Find(c => c.Id == guide.Id);
            int index = _tourGuides.IndexOf(current);
            _tourGuides.Remove(current);
            _tourGuides.Insert(index, guide);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _tourGuides);
            return guide;
        }
    }
}
