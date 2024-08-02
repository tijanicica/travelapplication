using System;
using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Dto;

public class TourDto
{
    public int Id { get; set; }
    
    public string FirstPhoto { get; set; }
    public string Name { get; set; }
    
    public DateTime StartDate { get; set; }
    public List<PersonOnTour> OtherPeopleOnTour  { get; set; }
    public string Language { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string TourGuide { get; set; }
    public string Description { get; set; }
    public string Duration { get; set; }
    
}