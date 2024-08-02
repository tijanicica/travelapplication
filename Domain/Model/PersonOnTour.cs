using System;
using ISerializable = BookingApp.Serializer.ISerializable;

namespace BookingApp.Domain.Model

{

    public class PersonOnTour : ISerializable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surename { get; set; }
        public int Age { get; set; }
        
        public bool Arrived { get; set;  }
        
        /*public virtual string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name, Surename, Age.ToString(), Arrived.ToString() };
            return csvValues;
        }*/
        public virtual string[] ToCSV()
        {
            string[] csvValues = { Id, Name, Surename, Age.ToString(), Arrived.ToString() };
            return csvValues;
        }

        /*public virtual void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Surename = values[2];
            Age = Convert.ToInt32(values[3]);
            Arrived = Convert.ToBoolean(values[4]);
        }*/
        
        public virtual void FromCSV(string[] values)
        {
            Id = values[0];
            Name = values[1];
            Surename = values[2];
            Age = Convert.ToInt32(values[3]);
            Arrived = Convert.ToBoolean(values[4]);
        }

    }
    
    


}