using System;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model
{
    public enum ReservationState { ACTIVE=0, CANCELED }
    public class AccommodationReservation : ISerializable
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int GuestNumber { get; set; }
        public int Duration { get; set; }
        
        public int GuestId { get; set; }
        
        public int AccommodationId { get; set; }

       // public Accommodation accommodation { get; set; }
      //  public bool RatedAccommodation { get; set; }
    //    public ReservationState State { get; set; }
      //  public Accommodation reservedAccommodation { get; set; }
      //  public bool isGuestRated { get; set; }
        public AccommodationReservation() { }

        public AccommodationReservation(DateTime startDate, DateTime endDate, int guestNumber, int duration, int accommodationId)
        {
            var app = System.Windows.Application.Current as App;
            StartDate = startDate;
            EndDate = endDate;
            GuestNumber = guestNumber;
            GuestId = app.LoggedUser.Id;
            Duration = duration;
            AccommodationId = accommodationId;
        }
        

        public AccommodationReservation(DateTime startDate, DateTime endDate, int days, int guestNumber, int accommodationId, int guestId)
        {
            StartDate = startDate;
            EndDate = endDate;
            Duration = days;
            GuestNumber = guestNumber;
            AccommodationId = accommodationId;
            GuestId = guestId;

        }
        public AccommodationReservation(int id, DateTime startDate, DateTime endDate, int guestNumber, int guestId, int duration, int accommodationId)
        {
            var app = System.Windows.Application.Current as App;

            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            GuestNumber = guestNumber;
            GuestId = app.LoggedUser.Id;
            Duration = duration;
            AccommodationId = accommodationId;
        }
        public AccommodationReservation(int id, DateTime startDate,  bool ratedA, bool ratedGuest, DateTime endDate, int guestNumber, int guestId, int duration, int accommodationId)
        {
            var app = System.Windows.Application.Current as App;

            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            GuestNumber = guestNumber;
            GuestId = app.LoggedUser.Id;
            Duration = duration;
            AccommodationId = accommodationId;
          //  RatedAccommodation = ratedA;
          //  isGuestRated = ratedGuest;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            StartDate = DateTime.Parse(values[1]);
            EndDate = DateTime.Parse(values[2]);
            GuestNumber = int.Parse(values[3]);
            Duration = int.Parse(values[4]);
            GuestId = int.Parse(values[5]);
            AccommodationId = int.Parse(values[6]);
           // RatedAccommodation = Convert.ToBoolean(values[7]);
           // isGuestRated = Convert.ToBoolean(values[8]);
          //  State = (ReservationState)Enum.Parse(typeof(ReservationState), values[9]);
        }

        public string[] ToCSV()
        {
            return new string[] {
                Id.ToString(),
                StartDate.ToString("yyyy-MM-dd"),
                EndDate.ToString("yyyy-MM-dd"),
                GuestNumber.ToString(),
                Duration.ToString(),
                GuestId.ToString(),
                AccommodationId.ToString()
                //RatedAccommodation.ToString(),
//isGuestRated.ToString(),
              //  State.ToString()
            };
        }
    }
}