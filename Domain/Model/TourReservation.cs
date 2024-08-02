using System;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model

{
    public class TourReservation : ISerializable
    {
        public int Id  { get; set; }
        
        public int TourExecutionId { get; set; }
        public int TouristId { get; set; }
        public int TouristNumber  { get; set; }
        public List<PersonOnTour> OtherPeopleOnTour  { get; set; }
        
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(), TourExecutionId.ToString(), TouristId.ToString(), TouristNumber.ToString(), String.Join(";" ,OtherPeopleOnTour.Select(e => String.Join(",",e.ToCSV())).ToArray())
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourExecutionId = Convert.ToInt32(values[1]);
            TouristId = Convert.ToInt32(values[2]);
            TouristNumber = Convert.ToInt32(values[3]);
            List<string> OtherPeopleOnTourRaw = values[4].Split(";").ToList();
            OtherPeopleOnTour = new List<PersonOnTour>();
            foreach (var e in OtherPeopleOnTourRaw)
            {
                PersonOnTour newPersonOnTour = new PersonOnTour();
                newPersonOnTour.FromCSV(e.Split(","));
                OtherPeopleOnTour.Add(newPersonOnTour);
            }
        }
    }
}