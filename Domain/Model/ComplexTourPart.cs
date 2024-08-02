using System;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model;
public enum ComplexTourPartStatus
{
    Pending,
    Accepted
};

public class ComplexTourPart : ISerializable
{
    public int Id { get; set; }
    public Location Location { get; set; }
    public string Description{ get; set; }
    public Language Language { get; set; }
    public  List<PersonOnTour> PeopleOnTour { get; set; }
    public int TouristId{ get; set; }
    public DateTime BeginDate { get; set; }
    public DateTime   EndDate { get; set; }
    public DateTime TourRequestDate { get; set; }
    public ComplexTourPartStatus Status { get; set; }
    public int TourGuideId { get; set; }
    public DateTime AcceptedDate { get; set; }
    
    public string[] ToCSV()
    {
        string[] csvValues =
        {
            Id.ToString(), String.Join(",", Location.ToCSV()), Description, Language.ToString()
            ,String.Join(";" ,PeopleOnTour.Select(e => String.Join(",",e.ToCSV())).ToArray()),
            TouristId.ToString(), BeginDate.ToString(), EndDate.ToString(), TourRequestDate.ToString(), Status.ToString(),
            TourGuideId.ToString(), AcceptedDate.ToString()
        };
        return csvValues;
    }

    public void FromCSV(string[] values)
    {
        Id = Convert.ToInt32(values[0]);
        Location = new Location();
        Location.FromCSV(values[1].Split(","));
        Description = values[2];
        Enum.TryParse<Language>(values[3], out var type);
        Language = type;
        List<string> people = values[4].Split(";").ToList();
        PeopleOnTour = new List<PersonOnTour>();
        foreach (var e in people)
        {
            PersonOnTour newPersonOnTour = new PersonOnTour();
            newPersonOnTour.FromCSV(e.Split(","));
            PeopleOnTour.Add(newPersonOnTour);
        }
        TouristId =  Convert.ToInt32(values[5]);
        BeginDate = DateTime.Parse(values[6]);
        EndDate = DateTime.Parse(values[7]);
        TourRequestDate = DateTime.Parse(values[8]);
        Enum.TryParse<ComplexTourPartStatus>(values[9], out var status);
        Status = status;
        TourGuideId = Convert.ToInt32((values[10]));
        AcceptedDate = DateTime.Parse(values[11]);

    }
    
    

}