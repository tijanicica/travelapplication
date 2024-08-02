using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.RepositoryInterfaces;

public interface IAccommodationRepository
{
        List<Accommodation> GetAll();
        Accommodation GetById(int id);
        Accommodation Save(Accommodation accommodation);
        int NextId();
        void Update(Accommodation accommodation, AccommodationReservation accommodationReservation);
        public void Delete(int accommodationId);
        List<Accommodation> GetAllByOwnersID(int ownerId);
        List<string> ParsePictures(List<string> photos);
        public List<string> GetImagePathsForAccommodation(int accommodationId);

}