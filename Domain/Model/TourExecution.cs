using System;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model

{
    public enum Status
    {
        Started,
        Finished,
        Inactive
    };

    public class TourExecution : ISerializable
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public Status Status { get; set; }
        public int CurrentTourSpotId { get; set; }
        public List<TourTourist> Tourists { get; set; }
        public DateTime StartDate { get; set; }

        public TourExecution()
        {
        }

        public TourExecution(int id, int tourId, Status status, int currentTourSpotId, List<TourTourist> tourists,
            DateTime startDate)
        {
            Id = id;
            TourId = tourId;
            Status = status;
            CurrentTourSpotId = currentTourSpotId;
            Tourists = tourists;
            StartDate = startDate;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(), TourId.ToString(), Status.ToString(), CurrentTourSpotId.ToString(),
                String.Join(";", Tourists.Select(tourist => String.Join(",", tourist.ToCSV())).ToArray()),
                StartDate.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourId = Convert.ToInt32(values[1]);
            Enum.TryParse<Status>(values[2], out var type);
            Status = type;
            CurrentTourSpotId = Convert.ToInt32(values[3]);
            Tourists = new List<TourTourist>();
            foreach (var tourist in values[4].Split(";"))
            {
                if (tourist == "")
                    break;
                TourTourist newTourist = new TourTourist();
                newTourist.FromCSV(tourist.Split(","));
                Tourists.Add(newTourist);
            }

            StartDate = DateTime.Parse(values[5]);
        }
    }
}