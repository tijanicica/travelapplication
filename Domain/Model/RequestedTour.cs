using System;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model;

public class RequestedTour : ISerializable
{
    public int Id { get; set; }
    public int TourRequestId { get; set; }
    public int TourGuideId { get; set; }
    public DateTime Date { get; set; }
    
    public string[] ToCSV()
    {
        string[] csvValues =
        {
            Id.ToString(), TourRequestId.ToString(), TourGuideId.ToString(), Date.ToString()
        };
        return csvValues;
    }
    
    public void FromCSV(string[] values)
    {
        Id = Convert.ToInt32(values[0]);
        TourRequestId = Convert.ToInt32(values[1]);
        TourGuideId = Convert.ToInt32(values[2]);
        Date = DateTime.Parse(values[3]);
     
    }

    
}