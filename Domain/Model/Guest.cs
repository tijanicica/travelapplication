using System;
using System.Collections.Generic;

namespace BookingApp.Domain.Model

{

    public class Guest : User
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