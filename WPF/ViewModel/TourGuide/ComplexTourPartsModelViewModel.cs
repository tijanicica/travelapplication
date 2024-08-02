namespace BookingApp.WPF.ViewModel.TourGuide;

public class ComplexTourPartsModelViewModel
{
    public int Id { get; set; }
    public string Language { get; set; }
    public string Location { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string Status { get; set; }

    public ComplexTourPartsModelViewModel(int id, string language, string location, string startDate, string endDate, string status)
    {
        Id = id;
        Language = language;
        Location = location;
        StartDate = startDate;
        EndDate = endDate;
        Status = status;
    }
}