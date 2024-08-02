using System;
using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Appl.UseCases;

public interface IAccommodationService
{ 
        Accommodation GetAccommodationById(int id);
        List<Accommodation> GetAllAccommodations();
        Accommodation Save(Accommodation accommodation);
        List<Accommodation> FilterAccommodations(string nameFilter, string locationFilter, string typeFilter,
            int maxGuestsFilter, int minDaysFilter);

        string GetAccommodationName(int accommodationId);
        void Delete(int accommodationId);
      

        public List<Accommodation> GetAllByOwnerID(int id);
        public List<Accommodation> GetAllByOwnersID(int id);




}