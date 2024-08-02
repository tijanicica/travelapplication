using System;
namespace BookingApp.Dto;

public class GuestDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    
    public string AccommodationName { get; set; }
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

}