using System;
using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Dto;

public class NotificationsDto
{
    public int Id { get; set; }
    public string TourGuideName { get; set; }
    public DateTime SentDateTime { get; set; }
    public int TouristId { get; set; }
    public int TourGuideId { get; set; }
    public int TourExecutionId { get; set; }
    public string TourExecutionName { get; set; }
    public DateTime TourExecutionDate { get; set; }
    public bool IsAccepted { get; set; }
    public bool IsSeen { get; set; }
    public List<PersonOnTour> MessageContent { get; set; }


    public NotificationsDto()
    {
        
    }

}