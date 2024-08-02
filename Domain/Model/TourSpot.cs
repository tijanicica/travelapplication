using System;
using ISerializable = BookingApp.Serializer.ISerializable;

namespace BookingApp.Domain.Model
{

    public class TourSpot : ISerializable
    {

        public int Id { get; set; }
        public string Description { get; set; }
        public bool Visited { get; set; }
        public bool Start { get; set; } = false;
        public bool End { get; set; } = false;

    public TourSpot(string description)

        {
            
            Visited = false;
        }

        public TourSpot()
        {
            
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Description, Visited.ToString(), Start.ToString(), End.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Int32.Parse(values[0]);
            Description = values[1];
            Visited = bool.Parse(values[2]);
            Start = bool.Parse(values[3]);
            End = bool.Parse(values[4]);
            
        }
    }

}