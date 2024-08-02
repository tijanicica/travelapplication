using System;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model;

public class GuestNotification : ISerializable
{
    public int Id { get; set; }
    public int ReservationId { get; set; }
    public DateTime NewDate { get; set; }
    public int GuestId { get; set; }
    public int OwnerId { get; set; }
    
    public string Username { get; set; }
    public string Answer { get; set; }
    
    public bool IsNotified { get; set; }

    public GuestNotification()
    {
        
    }
    public GuestNotification(int reservationId, DateTime newDate, int guestId, int ownerId, string username, string answer)
    {
        ReservationId = reservationId;
        NewDate = newDate;
        GuestId = guestId;
        OwnerId = ownerId;
        Username = username;
        Answer = answer;
        IsNotified = false;
    }

    public string[] ToCSV()
    {
        string[] csvValues = { Id.ToString(), ReservationId.ToString(), NewDate.ToString(), GuestId.ToString(), OwnerId.ToString(), Username, Answer, IsNotified.ToString()};
        return csvValues;
    }

    public void FromCSV(string[] values)
    {
        Id = Convert.ToInt32(values[0]);
        ReservationId = Convert.ToInt32(values[1]);
        NewDate = Convert.ToDateTime(values[2]);
        GuestId = Convert.ToInt32(values[3]);
        OwnerId = Convert.ToInt32(values[4]);
        Username = values[5];
        Answer = values[6];
        IsNotified = Convert.ToBoolean(values[7]);
    }
}