using System.Collections.Generic;
using System.IO;
using System.Linq;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;


namespace BookingApp.Repository
{
    public class GuestRepository : IGuestRepository
    {
        private const string FilePath = "../../../Resources/Data/guests.csv";

        private readonly Serializer<Guest> _serializer;

        private List<Guest> _guests;

        public GuestRepository()
        {
            _serializer = new Serializer<Guest>();
            _guests = _serializer.FromCSV(FilePath);
        }

        public User GetByUsername(string username)
        {
            _guests = _serializer.FromCSV(FilePath);
            return _guests.FirstOrDefault(u => u.Username == username);
        }
        
        public Guest Save(Guest guest)
        {
            guest.Id = NextId();
            _guests = _serializer.FromCSV(FilePath);
            _guests.Add(guest);
            _serializer.ToCSV(FilePath, _guests);
            return guest;
        }
        
        public int NextId()
        {
            _guests = _serializer.FromCSV(FilePath);
            if (_guests.Count < 1)
            {
                return 1;
            }
            return _guests.Max(c => c.Id) + 1;
        }


        

        public Guest GetById(int guestId)
        {
            List<Guest> guests = GetAll().ToList();
            foreach (Guest g in guests)
            {
                if (g.Id == guestId)
                {
                    return g;
                }

            }

            return null;
        }

        private void UpdateList()
        {
            _guests = _serializer.FromCSV(FilePath);
        }

        public IEnumerable<Guest> GetAll()
        {
            UpdateList();
            return _guests;
        }
        public string GetNameById(int id)
        {
            UpdateList();
            return GetById(id).Username;
                
        }

    }
   

}
