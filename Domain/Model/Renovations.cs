using System;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model;

public class Renovations : ISerializable
{
        public int Id { get; set; }
        public int AccommodationToRenovateId { get; set; }
        public string AccommodationToRenovateName {  get; set; }
        public DateTime WantedBeginingDate { get; set; }
        public DateTime WantedEndingDate { get; set; }
        public DateTime BeginingDate { get; set; }
        public DateTime EndingDate { get; set; }
        public string Description { get; set; }
        public int Length { get; set; }
        public Renovations() 
        {
            
        }

        public Renovations(int id, int accommodationToRenovateId, string accommodationToRenovateName, DateTime wantedBeginingDate, DateTime wantedEndingDate, DateTime beginingDate, DateTime endingDate, string description, int length)
        {
            Id = id;
            AccommodationToRenovateId = accommodationToRenovateId;
            AccommodationToRenovateName = accommodationToRenovateName;
            WantedBeginingDate = wantedBeginingDate;
            WantedEndingDate = wantedEndingDate;
            BeginingDate = beginingDate;
            EndingDate = endingDate;
            Description = description;
            Length = length;
        }

        public Renovations(int id, int accommodationToRenovateId, string accommodationToRenovateName, DateTime beginingDate, DateTime endingDate, string description, int length)
        {
            Id = id;
            AccommodationToRenovateId = accommodationToRenovateId;
            AccommodationToRenovateName = accommodationToRenovateName;
            BeginingDate = beginingDate;
            EndingDate = endingDate;
            Description = description;
            Length = length;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            AccommodationToRenovateId = int.Parse(values[1]);
            AccommodationToRenovateName = values[2];
            BeginingDate = DateTime.Parse(values[3]);
            // Ispravite indeks za EndingDate
            EndingDate = DateTime.Parse(values[4]);
            WantedBeginingDate = DateTime.Parse(values[5]);
            // Ispravite indeks za EndingDate
            WantedEndingDate = DateTime.Parse(values[6]);
            Description = values[7];
            Length = int.Parse(values[8]);
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AccommodationToRenovateId.ToString(),
                AccommodationToRenovateName,
                BeginingDate.ToString(),
                EndingDate.ToString(),
                WantedBeginingDate.ToString(),
                WantedEndingDate.ToString(),
                Description,
                Length.ToString(),
                
            };

            return csvValues;
        }
}
