using System;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model
{

    public class TourTourist : ISerializable
    {
        //public int Id { get; set; }
        public int TouristId { get; set; }
        public int JoinedAtTourSpot { get; set; }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
              TouristId.ToString(), JoinedAtTourSpot.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            //Id = Convert.ToInt32(values[0]);
            TouristId = Convert.ToInt32(values[0]);
            JoinedAtTourSpot = Convert.ToInt32(values[1]);
        }
    }

}