using System;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model
{
    public class OwnerNotification : ISerializable
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public DateTime DeletionDate { get; set; }
        public int GuestId { get; set; }
        public int OwnerId { get; set; } 

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), ReservationId.ToString(), DeletionDate.ToString(), GuestId.ToString(), OwnerId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            ReservationId = Convert.ToInt32(values[1]);
            DeletionDate = Convert.ToDateTime(values[2]);
            GuestId = Convert.ToInt32(values[3]);
            OwnerId = Convert.ToInt32(values[4]); 
        }
    }
}