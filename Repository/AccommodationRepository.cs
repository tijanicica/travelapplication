using BookingApp.Serializer;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;

namespace BookingApp.Repository
{
    public class AccommodationRepository : IAccommodationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodations.csv";

        private readonly Serializer<Accommodation> _serializer;
        private List<Accommodation> _accommodations;

        public AccommodationRepository()
        {
            _serializer = new Serializer<Accommodation>();
            _accommodations = _serializer.FromCSV(FilePath);
        }

        public List<Accommodation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        private void UpdateList()
        {
            _accommodations = _serializer.FromCSV(FilePath);
        }
        public Accommodation GetById(int id)
        {
            UpdateList();
            var accommodation = _accommodations.FirstOrDefault(t => t.Id == id);
            return accommodation;
        }
        public Accommodation Save(Accommodation accommodation)
        {
            accommodation.Id = NextId();
            _accommodations = _serializer.FromCSV(FilePath);
            _accommodations.Add(accommodation);
            _serializer.ToCSV(FilePath, _accommodations);
            return accommodation;
        }

        public int NextId()
        {
            _accommodations = _serializer.FromCSV(FilePath);
            if (_accommodations.Count < 1)
            {
                return 1;
            }

            return _accommodations.Max(c => c.Id) + 1;
        }

        


        public void Update(Accommodation accommodation, AccommodationReservation accommodationReservation)
        {
          

            if (!accommodation.Reservations.Any(r => r.Id == accommodationReservation.Id))
            {
                
                accommodation.Reservations.Add(accommodationReservation);

          
                //accommodation.ReservationIds = string.Join(", ", accommodationReservation.Id.ToString());
                accommodation.ReservationIds = "";

                _accommodations = _serializer.FromCSV(FilePath);
               
                
                var accommodationToUpdate = _accommodations.First(a => a.Id == accommodation.Id);
                
                foreach (Accommodation a in _accommodations)
                {
                    if (a.Id == accommodation.Id)
                    {
                        accommodationToUpdate.ReservationIds = accommodation.ReservationIds;
                        
                    }
                }
                
                _serializer.ToCSV(FilePath, _accommodations);
            }
        }
        public void Delete(int accommodationId)
        {
            var accommodationToRemove = _accommodations.FirstOrDefault(a => a.Id == accommodationId);
            if (accommodationToRemove != null)
            {
                _accommodations.Remove(accommodationToRemove);
                _serializer.ToCSV(FilePath, _accommodations);
            }
        }


            public List<Accommodation> GetAllByOwnersID(int ownerId)
            {
                List<Accommodation> ownersAccommodations = new List<Accommodation>();
                foreach(Accommodation a in _serializer.FromCSV(FilePath))
                {
                    if(a.OwnerId == ownerId)
                    {
                        ownersAccommodations.Add(a);
                    }
                }
                return ownersAccommodations;       
                
            }
            
            public List<string> ParsePictures(List<string> photos)
            {
                var picturePaths = new List<string>();

                foreach (string picture in photos)
                {
                    string relativePath = $"C:\\Users\\User\\Desktop\\sims-in-2024-group-1-team-a\\Resources\\Images/{picture}"; 
                    picturePaths.Add(relativePath);
                }

                return picturePaths;
            }
            
            public List<string> GetImagePathsForAccommodation(int accommodationId)
            {
                Accommodation accommodation = _accommodations.FirstOrDefault(a => a.Id == accommodationId);
                if (accommodation != null)
                {
                    // Vraćanje kopije liste putanja slika smeštaja
                    return new List<string>(accommodation.Photos);
                }
                else
                {
                    return new List<string>();
                }
            }
    }
}

