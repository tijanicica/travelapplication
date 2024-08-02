using System.Collections.Generic;
using System.Linq;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;

using BookingApp.Serializer;

namespace BookingApp.Repository
{

    public class RenovationsRepository : IRenovationsRepository
    {
        private const string FilePath = "../../../Resources/Data/renovations.csv";
        private readonly Serializer<Renovations> _serializer;
        private List<Renovations> renovations;

        public RenovationsRepository()
        {
            _serializer = new Serializer<Renovations>();
            renovations = _serializer.FromCSV(FilePath);
        }


        public List<Renovations> GetAll()
        {
            UpdateList();
            return renovations;
        }

        private void UpdateList()
        {
            renovations = _serializer.FromCSV(FilePath);
        }

        public Renovations Save(Renovations renovation)
        {
            renovation.Id = NextId();
            renovations = _serializer.FromCSV(FilePath);
            renovations.Add(renovation);
            _serializer.ToCSV(FilePath, renovations);
            return renovation;
        }

        public int NextId()
        {
            renovations = _serializer.FromCSV(FilePath);
            if (renovations.Count < 1)
            {
                return 1;
            }

            return renovations.Max(c => c.Id) + 1;
        }

        public void Delete(Renovations renovation)
        {
            renovations = _serializer.FromCSV(FilePath);
            Renovations founded = renovations.Find(c => c.Id == renovation.Id);
            renovations.Remove(founded);
            _serializer.ToCSV(FilePath, renovations);
        }

        public Renovations? removeRenovation(int idRen)
        {
            Renovations? rn = getRenovationByID(idRen);
            if (rn == null)
            {
                return null;
            }

            renovations = _serializer.FromCSV(FilePath);
            renovations.Remove(rn);
            _serializer.ToCSV(FilePath, renovations);
            return rn;
        }

        public Renovations? getRenovationByID(int idRen)
        {
            renovations = _serializer.FromCSV(FilePath);
            Renovations founded = renovations.Find(c => c.Id == idRen);
            return founded;
        }

        public Renovations Update(Renovations renovation)
        {
            renovations = _serializer.FromCSV(FilePath);
            Renovations current = renovations.Find(c => c.Id == renovation.Id);
            int index = renovations.IndexOf(current);
            renovations.Remove(current);
            renovations.Insert(index, renovation);
            _serializer.ToCSV(FilePath, renovations);
            return renovation;
        }
        
    }
}