using BookingApp.Serializer;

namespace BookingApp.Domain.Model
{
    public class Location : ISerializable
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public string[] ToCSV()

        {
            string[] csvValues = {  City, Country };

            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            if (values.Length >= 2)
            {
                City = values[0];
                Country = values[1];
            }
        }

        public Location( string city, string country)
        {
            City = city;
            Country = country;
        }

        public Location()
        {
            
        }
    }



}