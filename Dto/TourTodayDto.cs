using System;
using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Dto;

public class TourTodayDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public int TourGuideId { get; set; }
    
    public List<TourSpot> TourSpots { get; set; }
}