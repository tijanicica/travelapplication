using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BookingApp.Repository;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model
{
    public class Accommodation : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public AccommodationType Type { get; set; }
        public int MaxGuestNumber { get; set; }
        public int MinDuration { get; set; }
        public int? CancellationDeadline { get; set; }
        public List<string> Photos { get; set; }
        public int OwnerId { get; set; }
        
        
        
        public ObservableCollection<AccommodationReservation>? Reservations { get; set; }
        public string? ReservationIds 
        {
            get { return Reservations != null ? string.Join(", ", Reservations.Select(r => r.Id)) : null; }
            set 
            {
                if (Reservations == null)
                {
                    Reservations = new ObservableCollection<AccommodationReservation>();
                }
        
                if (!string.IsNullOrEmpty(value))
                {
                    string[] ids = value.Split(',').Select(id => id.Trim()).ToArray();
                    foreach (string id in ids)
                    {
                        if (int.TryParse(id, out int reservationId))
                        {
                            AccommodationReservation reservation = new AccommodationReservation();
                            reservation.Id = reservationId;
                            Reservations.Add(reservation);
                        }
                    }
                }
            }
        }



        
        public Accommodation()
        {
        }

        public Accommodation(string name, Location location, AccommodationType type, int maxGuestNumber, int minDuration, int cancellationDeadline, int ownerId)
        {
            Name = name;
            Location = location;
            Type = type;
            MaxGuestNumber = maxGuestNumber;
            MinDuration = minDuration;
            CancellationDeadline = cancellationDeadline;
            OwnerId = ownerId;
        }
        
        /*private string GetReservationIds()
        {
            if (Reservations != null && Reservations.Any())
            {
                List<string> reservationIds = new List<string>();
                foreach (var reservation in Reservations)
                {
                    reservationIds.Add(reservation.Id.ToString());
                }
                return string.Join(",", reservationIds);
            }
            else
            {
                return ""; // Vrati prazan string ako nema rezervacija ili je lista rezervacija null
            }
        }*/

        public string[] ToCSV()
        {
            List<string> csvValues = new List<string>
            {
                Id.ToString(),
                Name,
                String.Join(",", Location.ToCSV()),
                Type.ToString(),
                MaxGuestNumber.ToString(),
                MinDuration.ToString(),
                CancellationDeadline.ToString(),
                String.Join("*", Photos.ToArray()),
                OwnerId.ToString(),
                ReservationIds != null ? ReservationIds : " " 
            };
            return csvValues.ToArray();
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Name = values[1];
            string[] locationValues = values[2].Split(',');
            Location = new Location();
            Location.FromCSV(values[2].Split(","));
            Type = (AccommodationType)Enum.Parse(typeof(AccommodationType), values[3]);
            MaxGuestNumber = int.Parse(values[4]);
            MinDuration = int.Parse(values[5]);
            CancellationDeadline = int.Parse(values[6]);
            Photos = new List<string>(values[7].Split("*"));
            OwnerId = int.Parse(values[8]);
            LoadReservations(values[9]);
            
        }
        
        
        private void LoadReservations(string reservationsString)
        {
            string[] reservationStrings = reservationsString.Split(',');

            Reservations = new ObservableCollection<AccommodationReservation>();

            AccommodationReservationRepository repository = new AccommodationReservationRepository();

            foreach (string reservationIdString in reservationStrings)
            {
                int reservationId;
                if (int.TryParse(reservationIdString, out reservationId))
                {
                    AccommodationReservation reservation = repository.GetReservationById(reservationId);
                    if (reservation != null)
                    {
                        Reservations.Add(reservation);
                    }
                    else
                    {
                 
                    }
                }
            }
        }


       
      

    }
}
