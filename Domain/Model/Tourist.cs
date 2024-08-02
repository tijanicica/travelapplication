using System;

namespace BookingApp.Domain.Model

{

    public class Tourist : User
    {
        public override string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Username };
            return csvValues;
        }

        public override void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Username = values[1];
        }

    }


}