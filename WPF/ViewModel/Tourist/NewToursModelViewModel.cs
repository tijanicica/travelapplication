using System;

namespace BookingApp.WPF.ViewModel.Tourist;

public class NewToursModelViewModel
{
    public string StartDate { get; set; }
    public string TourGuideName { get; set; }
    public string Location { get; set; } 
    public string Language { get; set; }
    public string Description { get; set; }

    public NewToursModelViewModel(DateTime startDate, string tourGuideName, string location, string language, string description)
    {
        StartDate = startDate.ToString("dd/MM/yyyy");
        TourGuideName = tourGuideName;
        Location = location;
        Language = language;
        Description = description;
    }
}