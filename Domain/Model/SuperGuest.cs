using System;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model;

public class SuperGuest : ISerializable
{
    
    public int Id { get; set; }
    public int GuestId { get; set; }
    public int ReservationsNumber { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int BonusPoints { get; set; }
    public bool IsSuperGuestNextYear { get; set; }

    public SuperGuest()
    {
        
    }
    public SuperGuest(int id, int guestId, int reservationsNumber, DateTime startDate, DateTime endDate, int bonusPoints, bool isSuperGuest)
    {
        Id = id;
        GuestId = guestId;
        ReservationsNumber = reservationsNumber;
        StartDate = startDate;
        EndDate = endDate;
        BonusPoints = bonusPoints;
        IsSuperGuestNextYear = isSuperGuest;
    }

    public void FromCSV(string[] values)
    {
        Id = Convert.ToInt32(values[0]);
        GuestId = Convert.ToInt32(values[1]);
        ReservationsNumber = Convert.ToInt32(values[2]);
        StartDate = DateTime.Parse(values[3]);
        EndDate = DateTime.Parse(values[4]);
        BonusPoints = Convert.ToInt32(values[5]);
        IsSuperGuestNextYear = Convert.ToBoolean(values[6]);
    }

    public string[] ToCSV()
    {
        string[] strings = { Id.ToString(), GuestId.ToString(), ReservationsNumber.ToString(), StartDate.ToString(), EndDate.ToString(), BonusPoints.ToString(), IsSuperGuestNextYear.ToString() };
        return strings;
    }
}