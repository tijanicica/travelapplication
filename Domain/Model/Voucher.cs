using System;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model;


    public enum Reason
    {
        TourCancelled,
        GuideQuit,
        Awarded
    };

public class Voucher : ISerializable


{
    public int Id { get; set; }
    public int TouristId { get; set; }
    
    public bool IsValid { get; set; }
    public DateTime ExpirationDate { get; set; }
    public Reason Reason { get; set; }
    public int TourGuideId { get; set; }

  

    public Voucher()
    {
        
    }

    public string[] ToCSV()
    {
        string[] csvValues =
        {
            Id.ToString(), TouristId.ToString(), IsValid.ToString(), ExpirationDate.ToString(),
            Reason.ToString(), TourGuideId.ToString()
        };
        return csvValues;
    }

    public void FromCSV(string[] values)
    {
        Id = Convert.ToInt32(values[0]);
        TouristId = Convert.ToInt32(values[1]);
        IsValid = Convert.ToBoolean(values[2]);
        ExpirationDate = DateTime.Parse(values[3]);
        Enum.TryParse<Reason>(values[4], out var type);
        Reason = type;
        TourGuideId = Convert.ToInt32(values[5]);

    }
}