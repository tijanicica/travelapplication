using System;

namespace BookingApp.Domain.Model

{

    public class Owner : User
    {
        public bool IsSuperOwner { get; set; }
        
        public override string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Username, IsSuperOwner.ToString()};
            return csvValues;
        }

        public override void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Username = values[1];
            IsSuperOwner = Convert.ToBoolean(values[2]);
        }

    }


}