namespace BookingApp.WPF.ViewModel.Owner;

public class StatisticsModelViewModel: ViewModelBase
{
    public int AccommodationId { get; set; }
    public string AccommodationName { get; set; }
    public int ReservationsCount { get; set; }
    public int CancellationsCount { get; set; }
    public int ReschedulingCount {  get; set; }
    public int RenovationSuggestionsCount {  get; set; }
    public int Year {  get; set; }
    //public int Month { get; set; }
    public int sumOfDaysAccommodationWasOcupiedInAYear { get; set; }

    public StatisticsModelViewModel(int accommodationId, string accommodationName, int reservationsCount, int cancellationsCount, int reschedulingCount, int renovationSuggestionsCount, int year, int sumOfDaysAccommodationWasOcupiedInAYear)
    {
        AccommodationId = accommodationId;
        AccommodationName = accommodationName;
        ReservationsCount = reservationsCount;
        CancellationsCount = cancellationsCount;
        ReschedulingCount = reschedulingCount;
        RenovationSuggestionsCount = renovationSuggestionsCount;
        Year = year;
       // Month = month;
        this.sumOfDaysAccommodationWasOcupiedInAYear = sumOfDaysAccommodationWasOcupiedInAYear;
    }

    public StatisticsModelViewModel(int accommodationId, string accommodationName, int reservationsCount, int cancellationsCount, int reschedulingCount, int renovationSuggestionsCount, int year)
    {
        AccommodationId = accommodationId;
        AccommodationName = accommodationName;
        ReservationsCount = reservationsCount;
        CancellationsCount = cancellationsCount;
        ReschedulingCount = reschedulingCount;
        RenovationSuggestionsCount = renovationSuggestionsCount;
        Year = year;
    }
}