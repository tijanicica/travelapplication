namespace BookingApp.WPF.ViewModel.TourGuide;

public class JoinedTouristViewModel
{
    public string TouristName { get; set; }
    public string JoinedTourSpot { get; set; }

    public JoinedTouristViewModel()
    {
        
    }

    public JoinedTouristViewModel(string touristName, string joinedTourSpot)
    {
        TouristName = touristName;
        JoinedTourSpot = joinedTourSpot;
    }
}