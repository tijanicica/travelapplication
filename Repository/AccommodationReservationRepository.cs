using System;
using System.Collections.ObjectModel;
using System.Linq;
using BookingApp.Serializer;
using System.Collections.Generic;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.Repository
{
    public class AccommodationReservationRepository : IAccommodationReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationreservations.csv";

        private readonly Serializer<AccommodationReservation> _serializer;
        private ObservableCollection<AccommodationReservation> _accommodationReservations;

        public AccommodationReservationRepository()
        {
            _serializer = new Serializer<AccommodationReservation>();
            _accommodationReservations =
                new ObservableCollection<AccommodationReservation>(_serializer.FromCSV(FilePath));
        }

        public ObservableCollection<AccommodationReservation> GetAllReservations()
        {
            _accommodationReservations =
                new ObservableCollection<AccommodationReservation>(_serializer.FromCSV(FilePath));
            return _accommodationReservations;
        }

        public AccommodationReservation GetReservationById(int id)
        {
            return _accommodationReservations.FirstOrDefault(r => r.Id == id);
        }

        public void Save(AccommodationReservation accommodationReservation)
        {
            accommodationReservation.Id = NextId();
            _accommodationReservations =
                new ObservableCollection<AccommodationReservation>(_serializer.FromCSV(FilePath));
            _accommodationReservations.Add(accommodationReservation);
            _serializer.ToCSV(FilePath, _accommodationReservations.ToList());
        }

        public int NextId()
        {
            _accommodationReservations =
                new ObservableCollection<AccommodationReservation>(_serializer.FromCSV(FilePath));
            if (_accommodationReservations.Count < 1)
            {
                return 1;
            }

            return _accommodationReservations.Max(c => c.Id) + 1;
        }

        public List<AccommodationReservation> GetAllReservationsByAccomodationId(int accommodationId)
        {
            _accommodationReservations =
                new ObservableCollection<AccommodationReservation>(_serializer.FromCSV(FilePath));

            var reservationsByAccommodationId = _accommodationReservations
                .Where(reservation => reservation.AccommodationId == accommodationId).ToList();

            return reservationsByAccommodationId;
        }

        public void RemoveReservation(int reservationId)
        {
            var reservationToRemove = _accommodationReservations.FirstOrDefault(r => r.Id == reservationId);

            if (reservationToRemove != null)
            {
                _accommodationReservations.Remove(reservationToRemove);
                _serializer.ToCSV(FilePath, _accommodationReservations.ToList());
            }
            else
            {
                Console.WriteLine($"Reservation with ID {reservationId} not found.");
            }
        }

        public AccommodationReservation GetById(int reservationId)
        {
            var accommodation = _accommodationReservations.FirstOrDefault(t => t.Id == reservationId);
            return accommodation;
        }

        public AccommodationReservation Update(AccommodationReservation accommodationReservation)
        {
            _accommodationReservations = new ObservableCollection<AccommodationReservation>(_serializer.FromCSV(FilePath));
            AccommodationReservation current = _accommodationReservations.FirstOrDefault(c => c.Id == accommodationReservation.Id);
            if (current != null)
            {
                int index = _accommodationReservations.IndexOf(current);
                _accommodationReservations.Remove(current);
                _accommodationReservations.Insert(index, accommodationReservation); // keep ascending order of ids in file 
                _serializer.ToCSV(FilePath, _accommodationReservations.ToList());
            }
            return accommodationReservation;
        }

        public List<AccommodationReservation> GetByUser(User user)
        {
            var reservations = _serializer.FromCSV(FilePath);
            return reservations.FindAll(a => a.GuestId == user.Id);
        }
    }
}
