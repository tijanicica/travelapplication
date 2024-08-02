using System;
using BookingApp.Domain.Model;

namespace BookingApp.WPF.ViewModel.TourGuide;

public class AllTourRequestsModelViewModel : ViewModelBase


{
    public int Id { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Language { get; set; }
    public string BeginDate { get; set; }
    public string EndDate { get; set; }
    public string Status { get; set; }
    public string NoPeople { get; set; }
    public bool IsPending { get; set; }

    public AllTourRequestsModelViewModel(int id, string country, string city, string language, string beginDate, string endDate, string status, string noPeople)
    {
        Id = id;
        Country = country;
        City = city;
        Language = language;
        BeginDate = beginDate;
        EndDate = endDate;
        Status = status;
        NoPeople = noPeople;
        IsPending = status == TourRequestsStatus.Pending.ToString();
    }
}