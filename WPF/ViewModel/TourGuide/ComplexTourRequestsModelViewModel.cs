namespace BookingApp.WPF.ViewModel.TourGuide;

public class ComplexTourRequestsModelViewModel
{
    public int Id { get; set; }
    public string ComplexTourLabel { get; set; }

    public ComplexTourRequestsModelViewModel(string complexTourLabel, int id)
    {
        ComplexTourLabel = complexTourLabel;
        Id = id;
    }
}