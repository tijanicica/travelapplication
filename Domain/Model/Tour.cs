using System;
using System.Collections.Generic;
using System.Linq;
using ISerializable = BookingApp.Serializer.ISerializable;
using System;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Serializer;


namespace BookingApp.Domain.Model

{
    public enum Language
    {
        English,
        Serbian,
        Spanish,
        French
    };

    public class Tour : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }
        public Language Language { get; set; }
        public int MaxTouristNumber { get; set; }
        public List<TourSpot> TourSpots { get; set; }
        public double Duration { get; set; }
        public List<string> Photos { get; set; }
        public int TourGuideId { get; set; }


        public Tour(int id, string name, Location location, string description, Language language, int maxTouristNumber,
            List<TourSpot> tourSpots, double duration, List<string> photos, int tourGuideId)
        {
            Id = id;
            Name = name;
            Location = location;
            Description = description;
            Language = language;
            MaxTouristNumber = maxTouristNumber;
            TourSpots = tourSpots;
            Duration = duration;
            Photos = photos;
            TourGuideId = tourGuideId;
        }

        public Tour(string name, Location location, string description, Language language, int maxTouristNumber,
            double duration)
        {
            Name = name;
            Location = location;
            Description = description;
            Language = language;
            MaxTouristNumber = maxTouristNumber;
            Duration = duration;
        }

        public Tour()
        {
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(), Name, String.Join(",", Location.ToCSV()), Description, Language.ToString(),
                MaxTouristNumber.ToString(),
                String.Join(";", TourSpots.Select(spot => String.Join(",", spot.ToCSV())).ToArray()),
                Duration.ToString(), String.Join("*", Photos.ToArray()), TourGuideId.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Location = new Location();
            Location.FromCSV(values[2].Split(","));
            Description = values[3];
            Enum.TryParse<Language>(values[4], out var type);
            Language = type;
            MaxTouristNumber = Int32.Parse(values[5]);
            List<string> tourSpotsRaw = values[6].Split(";").ToList();
            TourSpots = new List<TourSpot>();
            foreach (var spot in tourSpotsRaw)
            {
                TourSpot newTourSpot = new TourSpot();
                newTourSpot.FromCSV(spot.Split(","));
                TourSpots.Add(newTourSpot);
            }

            Duration = Double.Parse(values[7]);
            Photos = new List<string>(values[8].Split("*"));
            TourGuideId = Convert.ToInt32(values[9]);
        }
    }


}