using System;
using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Dto;


public class TourDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public DateTime StartDate { get; set; }
    public Location Location { get; set; }
    public string Description { get; set; }
    public Language Language { get; set; }
    public int MaxTouristNumber { get; set; }
    public List<TourSpot> TourSpots { get; set; }
    public double Duration { get; set; }
    public List<string> Photos { get; set; }
}