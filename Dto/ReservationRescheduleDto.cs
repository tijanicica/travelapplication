using System;
using BookingApp.Domain.Model;

namespace BookingApp.Dto;

public class ReservationRescheduleDto
{
    public int Id { get; set; }
    public int GuestId { get; set; }
    public int ReservationId { get; set; }
    public int AccommodationId { get; set; }
    public string Available { get; set; }
    public string Username { get; set; }
    public string NameAccomodation { get; set; }
    public DateTime NewStartDate { get; set; }
    public DateTime NewEndDate { get; set; }
    public ReschedulingStatus ReschedulingAnswerStatus { get; set; }
    public string RejectionComment { get; set; } 
    public int OwnerId { get; set; }

}