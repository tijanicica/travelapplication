using System;
using BookingApp.Repository;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.Service
{
    public class AccommodationService : IAccommodationService
    {
        private readonly IAccommodationRepository _accommodationRepository;
      // private readonly IAccommodationReservationService _accommodationReservationService;
        
        public AccommodationService(IAccommodationRepository accommodationRepository)
        {
            _accommodationRepository = accommodationRepository;
          //  _accommodationReservationService = accommodationReservationService;
        }
        public Accommodation GetAccommodationById(int id)
        {
            return _accommodationRepository.GetById(id);
        }
        public List<Accommodation> GetAllAccommodations()
        {
            return _accommodationRepository.GetAll();
        }

        public Accommodation Save(Accommodation accommodation)
        {
            return _accommodationRepository.Save(accommodation);
        }

        public List<Accommodation> FilterAccommodations(string nameFilter, string locationFilter, string typeFilter, int maxGuestsFilter, int minDaysFilter)
        {
            var accommodations = _accommodationRepository.GetAll();

            accommodations = FilterByName(accommodations, nameFilter);
            accommodations = FilterByLocation(accommodations, locationFilter);
            accommodations = FilterByType(accommodations, typeFilter);
            accommodations = FilterByMaxGuests(accommodations, maxGuestsFilter);
            accommodations = FilterByMinDays(accommodations, minDaysFilter);

            return accommodations;
        }

        private List<Accommodation> FilterByName(List<Accommodation> accommodations, string nameFilter)
        {
            if (!string.IsNullOrEmpty(nameFilter))
                return accommodations.Where(acc => acc.Name.ToLower().Contains(nameFilter.ToLower())).ToList();

            return accommodations;
        }

        private List<Accommodation> FilterByLocation(List<Accommodation> accommodations, string locationFilter)
        {
            if (!string.IsNullOrEmpty(locationFilter))
            {
                var parts = locationFilter.Split(',');
                var cityFilter = parts[0].Trim().ToLower();
                var countryFilter = parts.Length > 1 ? parts[1].Trim().ToLower() : null;

                return accommodations.Where(acc =>
                {
                    if (acc.Location == null)
                        return false;

                    var cityMatches = acc.Location.City.ToLower().Contains(cityFilter);
                    var countryMatches = string.IsNullOrEmpty(countryFilter) || acc.Location.Country.ToLower().Contains(countryFilter);

                    return cityMatches && countryMatches;
                }).ToList();
            }

            return accommodations;
        }

        private List<Accommodation> FilterByType(List<Accommodation> accommodations, string typeFilter)
        {
            if (!string.IsNullOrEmpty(typeFilter))
            {
                var filteredTypes = typeFilter.ToLower().Split(',').Select(t => t.Trim());
                return accommodations.Where(acc => filteredTypes.Any(ft => acc.Type.ToString().ToLower().Contains(ft))).ToList();
            }

            return accommodations;
        }



        private List<Accommodation> FilterByMaxGuests(List<Accommodation> accommodations, int maxGuestsFilter)
        {
            if (maxGuestsFilter > 0)
                return accommodations.Where(acc => acc.MaxGuestNumber >= maxGuestsFilter).ToList();

            return accommodations;
        }

        private List<Accommodation> FilterByMinDays(List<Accommodation> accommodations, int minDaysFilter)
        {
            if (minDaysFilter > 0)
                return accommodations.Where(acc => acc.MinDuration >= minDaysFilter).ToList();

            return accommodations;
        }
        
        public string GetAccommodationName(int accommodationId)
        {
            var accommodation = _accommodationRepository.GetById(accommodationId);
        
            return accommodation != null ? accommodation.Name : string.Empty;
      }
        public List<Accommodation> GetAllByOwnersID(int ownerId)
        {
            return _accommodationRepository.GetAllByOwnersID(ownerId);
        }
        
        public void Delete(int accommodationId)
        {
           _accommodationRepository.Delete(accommodationId);
        }
        //promeniti cirkularna zavisnost prebaciti u accommodationReservationService i izbaciti iz AccommodationServica AccmomdationReswervation
      
        public List<Accommodation> GetAllByOwnerID(int id)
        {
            List<Accommodation> accommodations = new List<Accommodation>();
            foreach(Accommodation a in _accommodationRepository.GetAll())
            {
                if(a.Id == id)
                {
                    accommodations.Add(a);
                }
            }
            return accommodations;
        }
        
     

    }
    
}
