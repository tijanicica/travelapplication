using System;

namespace BookingApp.WPF.ViewModel.Tourist;

public class NotificationRegularTourModelViewModel
{
    public string Location { get; set; }
    
    public string TourGuideName { get; set; }
    public string Date { get; set; }

    public NotificationRegularTourModelViewModel(string location, string tourGuideName, DateTime date)
    {
        Location = location;
        TourGuideName = tourGuideName;
        Date = date.ToString("dd/MM/yyyy");
    }
}