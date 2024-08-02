using BookingApp.Serializer;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private const string FilePath = "../../../Resources/Data/owners.csv";

        private readonly Serializer<Owner> _serializer;

        private List<Owner> _owners;

        public OwnerRepository()
        {
            _serializer = new Serializer<Owner>();
            _owners = _serializer.FromCSV(FilePath);
        }

        public User GetByUsername(string username)
        {
            _owners = _serializer.FromCSV(FilePath);
            return _owners.FirstOrDefault(u => u.Username == username);
        }
        
        public void UpdateIsSuperOwner(int ownerId, bool isSuperOwner)
        {
            var owner = _owners.FirstOrDefault(u => u.Id == ownerId);
            if (owner != null)
            {
                owner.IsSuperOwner = isSuperOwner;

                // Sačuvaj ažurirane vlasnike u CSV fajl
                _serializer.ToCSV(FilePath, _owners);
            }
        }
        
        public List<Owner> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Owner GetById(int id)
        {
            var owner = _owners.FirstOrDefault(u => u.Id == id);
            return owner;
        }


    }
}
