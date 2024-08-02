using System;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model;

public class ReservationReschedule : ISerializable
{

    public int Id { get; set; }
    public int GuestId { get; set; }
    public int ReservationId { get; set; }
    public int AccommodationId { get; set; }
    public DateTime NewStartDate { get; set; }
    public DateTime NewEndDate { get; set; }
    public ReschedulingStatus ReschedulingAnswerStatus { get; set; }

    public string RejectionComment { get; set; } 
    public int OwnerId { get; set; }

    public ReservationReschedule()
    {
        
    }
    public ReservationReschedule(int guestId, int reservationId, int accommodationId, DateTime newStartDate, DateTime newEndDate, ReschedulingStatus reschedulingAnswerStatus, string rejectionComment, int ownerId)
    {
        
        OwnerId = ownerId;
        GuestId = guestId;
        ReservationId = reservationId;
        AccommodationId = accommodationId;
        NewStartDate = newStartDate;
        NewEndDate = newEndDate;
        ReschedulingAnswerStatus = reschedulingAnswerStatus;
        RejectionComment = rejectionComment;
    }



    public string[] ToCSV()
    {
        return new string[] {
            Id.ToString(),
            GuestId.ToString(),
            ReservationId.ToString(),
            AccommodationId.ToString(),
            NewStartDate.ToString("yyyy-MM-dd"),
            NewEndDate.ToString("yyyy-MM-dd"),
            ReschedulingAnswerStatus.ToString(),
            RejectionComment,
            OwnerId.ToString()
        };
        
    }

    public void FromCSV(string[] values)
    {
        Id = int.Parse(values[0]);
        GuestId = int.Parse(values[1]);
        ReservationId = int.Parse(values[2]);
        AccommodationId = int.Parse(values[3]);
        NewStartDate = DateTime.Parse(values[4]);
        NewEndDate = DateTime.Parse(values[5]);
        ReschedulingAnswerStatus = (ReschedulingStatus)Enum.Parse(typeof(ReschedulingStatus), values[6]);
        RejectionComment = values[7];
        OwnerId = int.Parse(values[8]);
    }
}