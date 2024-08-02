using System;
using System.Collections.Generic;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model;

public class TourReview : ISerializable
{
    public int Id { get; set; }
    public int TouristId { get; set; }
    
    public int TourExecutionId { get; set; }
    
    public int TourGuideId { get; set; }
    public int GuidesKnowledge { get; set; }
    public int GuidesLanguageSkills { get; set; }
    public int AmusementLevel { get; set; }
    public string Comment { get; set; }
    public List<string> Photos { get; set; }
    public bool IsValid { get; set; }

    public TourReview()
    {
        
    }
    
    public string[] ToCSV()
    {
        string[] csvValues =
        {
            Id.ToString(), TouristId.ToString(), TourExecutionId.ToString(), TourGuideId.ToString() , GuidesKnowledge.ToString(), GuidesLanguageSkills.ToString(), AmusementLevel.ToString(),
            Comment, String.Join("*", Photos.ToArray()), IsValid.ToString()
         
        };
        return csvValues;
    }

    public void FromCSV(string[] values)
    {
        Id = Convert.ToInt32(values[0]);
        TouristId = Convert.ToInt32(values[1]);
        TourExecutionId = Convert.ToInt32(values[2]);
        TourGuideId = Convert.ToInt32(values[3]);
        GuidesKnowledge = Convert.ToInt32(values[4]);
        GuidesLanguageSkills = Convert.ToInt32(values[5]);
        AmusementLevel = Convert.ToInt32(values[6]);
        Comment = values[7];
        Photos = new List<string>(values[8].Split("*"));
        IsValid = Convert.ToBoolean(values[9]);




    }


}

