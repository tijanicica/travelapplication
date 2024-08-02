namespace BookingApp.WPF.ViewModel.TourGuide;

public class TourReviewsViewModel : ViewModelBase
{
    public int ReviewId { get; set; }
    public string TouristName  { get; set; }
    public string TourName  { get; set; }
    public string JoinedAt  { get; set; }
    private bool _isValid;
    
    
    public bool Valid
    {
        get => _isValid;


        set
        {
            if (value != _isValid)
            {
                _isValid = value;
                OnPropertyChanged();
            }
        } }

    public TourReviewsViewModel(int reviewId, string touristName, string tourName, string joinedAt, bool valid)
    {
        ReviewId = reviewId;
        TouristName = touristName;
        TourName = tourName;
        JoinedAt = joinedAt;
        Valid = valid;
    }
    public TourReviewsViewModel()
    {
        
    }
    
}