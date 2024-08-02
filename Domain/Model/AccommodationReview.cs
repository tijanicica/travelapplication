using System;
using System.Collections.Generic;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model
{
    public class AccommodationReview : ISerializable
    {

        public int RatingID { get; set; } //id same ocene koju gost daje smestaju i vlasniku
        public int GuestID { get; set; } //samo se prosledi id ulogovanog gosta
        public int OwnerID { get; set; }
        public string GuestName { get; set; }
        public int AccommodationCleanliness { get; set; }
        public int OwnerCorrectness { get; set; }
        public string AdditionalComment { get; set; }
        
        public List<string> Photos { get; set; } //ovo nez kako da se cuva
        
        public string RenovationRecommendation { get; set; } // Tekstualni opis preporuke za renoviranje
        public int RenovationUrgencyLevel { get; set; } 
        public int AccommodationID { get; set; }
        public DateTime ReviewDate { get; set; }

        public AccommodationReview()
        {
        }

        public AccommodationReview(int guestId, int ownerId, string guestName, int accommodationCleanliness, int ownerCorrectness, string additionalComment, List<string> photos, string renovationRecommendation, int renovationUrgencyLevel)
        {
            GuestID = guestId;
            OwnerID = ownerId;
            GuestName = guestName;
            AccommodationCleanliness = accommodationCleanliness;
            OwnerCorrectness = ownerCorrectness;
            AdditionalComment = additionalComment;
            Photos = photos;
            RenovationRecommendation = renovationRecommendation;
            RenovationUrgencyLevel = renovationUrgencyLevel;
        }

        public AccommodationReview(int ratingId, int guestId, int ownerId, string guestName, int accommodationCleanliness, int ownerCorrectness, string additionalComment, List<string> photos, string renovationRecommendation, int renovationUrgencyLevel, int accommodationId, DateTime reviewDate)
        {
            RatingID = ratingId;
            GuestID = guestId;
            OwnerID = ownerId;
            GuestName = guestName;
            AccommodationCleanliness = accommodationCleanliness;
            OwnerCorrectness = ownerCorrectness;
            AdditionalComment = additionalComment;
            Photos = photos;
            RenovationRecommendation = renovationRecommendation;
            RenovationUrgencyLevel = renovationUrgencyLevel;
            AccommodationID = accommodationId;
            ReviewDate = reviewDate;
        }

        public void FromCSV(string[] values)
        {
            RatingID = Convert.ToInt32(values[0]);
            GuestID = Convert.ToInt32(values[1]);
            OwnerID = Convert.ToInt32(values[2]);
            GuestName = values[3];
            AccommodationCleanliness = Convert.ToInt32(values[4]);
            OwnerCorrectness = Convert.ToInt32(values[5]);
            AdditionalComment = values[6];
            Photos = new List<string>(values[7].Split("*"));
            
            RenovationRecommendation = values[8];
            RenovationUrgencyLevel = Convert.ToInt32(values[9]);
            AccommodationID = Convert.ToInt32(values[10]);
            ReviewDate = DateTime.Parse(values[11]);

        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                RatingID.ToString(),
                GuestID.ToString(),
                OwnerID.ToString(),
                GuestName,
                AccommodationCleanliness.ToString(),
                OwnerCorrectness.ToString(),
                AdditionalComment,
                String.Join("*", Photos.ToArray()),
                RenovationRecommendation,
                RenovationUrgencyLevel.ToString(),
                AccommodationID.ToString(),
                ReviewDate.ToString("yyyy-MM-dd"),
            };
            return csvValues;
        }
    }
}