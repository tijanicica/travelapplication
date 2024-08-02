using BookingApp.Serializer;

namespace BookingApp.Domain.Model;

public class StatisticsAccommodation : ISerializable
{
    public int AccommodationId { get; set; }
    public string AccommodationName { get; set; }
    public int ReservationsCount { get; set; }
    public int CancellationsCount { get; set; }
    public int ReschedulingCount {  get; set; }
    public int RenovationSuggestionsCount {  get; set; }
    public int Year {  get; set; }
    public int Month { get; set; }
    public int sumOfDaysAccommodationWasOcupiedInAYear { get; set; }
    public int sumOfDaysAccommodationWasOcupiedInAMonth { get; set; }

    public StatisticsAccommodation(int accommodationId, string accommodationName, int reservationsCount, int cancellationsCount, int reschedulingCount, int renovationSuggestionsCount, int year, int month, int sumOfDaysAccommodationWasOcupiedInAYear, int sumOfDaysAccommodationWasOcupiedInAMonth)
    {
        AccommodationId = accommodationId;
        AccommodationName = accommodationName;
        ReservationsCount = reservationsCount;
        CancellationsCount = cancellationsCount;
        ReschedulingCount = reschedulingCount;
        RenovationSuggestionsCount = renovationSuggestionsCount;
        Year = year;
        Month = month;
        this.sumOfDaysAccommodationWasOcupiedInAYear = sumOfDaysAccommodationWasOcupiedInAYear;
        this.sumOfDaysAccommodationWasOcupiedInAMonth = sumOfDaysAccommodationWasOcupiedInAMonth;
    }
    public StatisticsAccommodation(int accommodationId, string accommodationName, int reservationsCount, int cancellationsCount, int reschedulingCount, int renovationSuggestionsCount, int year, int month, int sumOfDaysAccommodationWasOcupiedInAYear)
    {
        AccommodationId = accommodationId;
        AccommodationName = accommodationName;
        ReservationsCount = reservationsCount;
        CancellationsCount = cancellationsCount;
        ReschedulingCount = reschedulingCount;
        RenovationSuggestionsCount = renovationSuggestionsCount;
        Year = year;
        Month = month;
        this.sumOfDaysAccommodationWasOcupiedInAYear = sumOfDaysAccommodationWasOcupiedInAYear;
    
    }
    public StatisticsAccommodation(int accommodationId, string accommodationName, int reservationsCount, int cancellationsCount, int reschedulingCount, int year, int month, int sumOfDaysAccommodationWasOcupiedInAYear)
    {
        AccommodationId = accommodationId;
        AccommodationName = accommodationName;
        ReservationsCount = reservationsCount;
        CancellationsCount = cancellationsCount;
        ReschedulingCount = reschedulingCount;
        Year = year;
        Month = month;
        this.sumOfDaysAccommodationWasOcupiedInAYear = sumOfDaysAccommodationWasOcupiedInAYear;
    }

    public StatisticsAccommodation()
    {
        
    }
    public string[] ToCSV()
    {
        return new string[] {
            AccommodationId.ToString(),
            AccommodationName,
            ReservationsCount.ToString(),
            CancellationsCount.ToString(),
            ReschedulingCount.ToString(),
           RenovationSuggestionsCount.ToString(),
            Year.ToString(),
            Month.ToString(),
            sumOfDaysAccommodationWasOcupiedInAYear.ToString(),
            sumOfDaysAccommodationWasOcupiedInAMonth.ToString()
        };
        
    }

    public void FromCSV(string[] values)
    {
        AccommodationId = int.Parse(values[0]);
        AccommodationName = values[1];
        ReservationsCount = int.Parse(values[2]);
        CancellationsCount = int.Parse(values[3]);
        ReschedulingCount = int.Parse(values[4]);
       RenovationSuggestionsCount = int.Parse(values[5]);
        Year = int.Parse(values[6]);
        Month = int.Parse(values[7]);
        sumOfDaysAccommodationWasOcupiedInAYear = int.Parse(values[8]);
        sumOfDaysAccommodationWasOcupiedInAMonth = int.Parse(values[9]);
        
    }
}