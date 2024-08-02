using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BookingApp.Repository;
using BookingApp.Serializer;
namespace BookingApp.Domain.Model;

public class Forum: ISerializable
{
        public int ForumId {  get; set; }
        public string GuestName { get; set; }
      //  public int LocationId { get; set; }
        public Location Location { get; set; }
        public bool HasOwnerSeenTheNotification { get; set; } //moje

        //u csv ce se svi komentari cuvati sa * izmedju npr "Anica je los smestaj*Ajahuaskovanje je solidno" itd... 
        public string AllOwnerCommentsUnsplited { get; set; } //moraju valjda da se pamte vlasnikovi i gostovi komentari odvojeno pa zato ovako
        public string AllGuestCommentsUnsplited { get; set; }
        public List<string> AllOwnerCommentsSplited { get; set; }
        public List<string> AllGuestCommentsSplited { get; set; }
        public int NumberOfOwnerComments {  get; set; }
        public int NumberOfGuestComments { get; set; }

        public Forum() { }

        public Forum(int forumID, Location Location, bool hasOwnerSeenTheNotification,
                     string allOwnerCommentsUnsplited, string allGuestCommentsUnsplited,
                     List<string> allOwnerCommentsSplited, List<string> allGuestCommentsSplited)
        {
            this.ForumId = forumID;
            this.Location = Location;
            //this.location = location;
            this.HasOwnerSeenTheNotification = hasOwnerSeenTheNotification;
            this.AllOwnerCommentsUnsplited = allOwnerCommentsUnsplited;
            this.AllGuestCommentsUnsplited = allGuestCommentsUnsplited;
            this.AllOwnerCommentsSplited = allOwnerCommentsSplited;
            this.AllGuestCommentsSplited = allGuestCommentsSplited;
        }

        public void FromCSV(string[] values)
        {
            //LocationService _locationService = new LocationService();
            ForumId = int.Parse(values[0]);
            string[] locationValues = values[1].Split(',');
            Location = new Location();
            Location.FromCSV(values[1].Split(","));
            HasOwnerSeenTheNotification = bool.Parse(values[2]);
            AllOwnerCommentsUnsplited = values[3];
            AllOwnerCommentsSplited = values[3].Split('*').ToList();
            AllGuestCommentsUnsplited = values[4];
            AllGuestCommentsSplited = values[4].Split('*').ToList();
            //location = _locationService.GetLocationByLocationID(locationID);
            NumberOfOwnerComments = AllOwnerCommentsSplited.Count;
            NumberOfGuestComments = AllGuestCommentsSplited.Count;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                ForumId.ToString(),
                String.Join(",", Location.ToCSV()),
                HasOwnerSeenTheNotification.ToString(),
                AllOwnerCommentsUnsplited.ToString(),
                AllGuestCommentsUnsplited.ToString(),
            };
            return csvValues;
        }
}