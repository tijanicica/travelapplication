using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BookingApp.Domain.Model;

namespace BookingApp.WPF.ViewModel.Tourist;

public class RegularTourRequestModelViewModel
{
    public string Location { get; set; }
   // public string Description { get; set; }
  //  public string Language { get; set; }
    //public ObservableCollection<PersonOnTourViewModel> PeopleOnTour { get; set; }
 //   public int TouristId { get; set; }
    public string BeginDate { get; set; }
    public string EndDate { get; set; }

    public string Status { get; set; }
    public string TourGuideName { get; set; }

    public RegularTourRequestModelViewModel(string location, DateTime  beginDate, DateTime  endDate, string status, string tourGuideName)
    {
        Location = location;
        BeginDate = beginDate.ToString("dd/MM/yyyy");
        EndDate = endDate.ToString("dd/MM/yyyy");
        Status = status;
        TourGuideName = tourGuideName; 
    }
}