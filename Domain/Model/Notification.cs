using System;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model;

public class Notification : ISerializable
{
    public int Id { get; set; }
    public int TouristId { get; set; }
    public int TourGuideId { get; set; }
    public int TourExecutionId { get; set; }
    public DateTime SentDateTime { get; set; }
    public bool IsAccepted { get; set; }
    public bool IsSeen { get; set; }
    public List<PersonOnTour> MessageContent { get; set; }

    public Notification()
    {
        
    }
    
    public string[] ToCSV()
    {
        string[] csvValues =
        {
            Id.ToString(), TouristId.ToString(), TourGuideId.ToString(), TourExecutionId.ToString(),
            SentDateTime.ToString(), IsAccepted.ToString(), IsSeen.ToString()
            
            ,String.Join(";" ,MessageContent.Select(e => String.Join(",",e.ToCSV())).ToArray())
        };
        return csvValues;
    }

    public void FromCSV(string[] values)
    {
        Id = Convert.ToInt32(values[0]);
        TouristId = Convert.ToInt32(values[1]);
        TourGuideId = Convert.ToInt32(values[2]);
        TourExecutionId = Convert.ToInt32(values[3]);
        SentDateTime = DateTime.Parse(values[4]);
        IsAccepted = Convert.ToBoolean(values[5]);
        IsSeen = Convert.ToBoolean(values[6]);
        List<string> messageContetRaw = values[7].Split(";").ToList();
        MessageContent = new List<PersonOnTour>();
        foreach (var e in messageContetRaw)
        {
            PersonOnTour newPersonOnTour = new PersonOnTour();
            newPersonOnTour.FromCSV(e.Split(","));
            MessageContent.Add(newPersonOnTour);
        }
    }
}