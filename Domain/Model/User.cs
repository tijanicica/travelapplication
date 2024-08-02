using System;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model
{
    public enum UserType { Owner = 0, Guest, TourGuide, Tourist};    

    public class User : ISerializable
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserType Type {  get; set; }


        public User() { }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public virtual string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Username, Password, Type.ToString() };
            return csvValues;
        }

        public virtual void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Username = values[1];
            Password = values[2];
            Enum.TryParse<UserType>(values[3], out var type);
            Type = type;
        }
    }
}
