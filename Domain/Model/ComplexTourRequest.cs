using System;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model;

public enum ComplexTourRequestStatus
{
    Pending,
    Accepted,
    Invalid
};

public class ComplexTourRequest : ISerializable
{
    public int Id { get; set; }
    public List<int> tourParts { get; set; }
    public ComplexTourRequestStatus Status { get; set; }
    
    public int TouristId { get; set; }
    
    
    public string[] ToCSV()
    {
        string[] csvValues =
        {
            Id.ToString(), String.Join(",", tourParts), Status.ToString(), TouristId.ToString()
        };
        return csvValues;
    }
    
    public void FromCSV(string[] values)
    {
        Id = Convert.ToInt32(values[0]);
        tourParts = values[1]
            .Split(',')
            .Select(int.Parse)
            .ToList();
        Enum.TryParse<ComplexTourRequestStatus>(values[2], out var status);
        Status = status;
        TouristId = Convert.ToInt32(values[3]);

    }
}