using System;

namespace BookingApp.Domain.Model

{
    public class TourGuide : User
    {

        public bool IsSuperGuide { get; set; }
        public override string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Username, IsSuperGuide.ToString() };
            return csvValues;
        }

        public override void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Username = values[1];
            IsSuperGuide = Convert.ToBoolean(values[2]);
        }
    }
}