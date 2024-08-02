using System;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model
{
    public class GuestReview : ISerializable
    {
        public int Id{ get; set; }
        public int GuestId{ get; set; }
        public int Cleanliness{ get; set; }
        public int RuleFollowing{ get; set; }
        public string Comment{ get; set; }
        public int OwnerId { get; set; }
        public int ReservationId { get; set; }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(), GuestId.ToString(), Cleanliness.ToString(), RuleFollowing.ToString(),Comment,  OwnerId.ToString(), ReservationId.ToString()
                
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuestId = Convert.ToInt32(values[1]);
            Cleanliness = Convert.ToInt32(values[2]);
            RuleFollowing = Convert.ToInt32(values[3]);
            Comment = values[4];
            OwnerId = Convert.ToInt32(values[5]);
            ReservationId = Convert.ToInt32(values[6]);
        }
    }

}