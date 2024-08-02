namespace BookingApp.WPF.ViewModel.Owner;

public class RenovationModelViewModel: ViewModelBase
{
    public int Id { get; set; }
    
    public int AccommodationToRenovateId { get; set; }
    
    public string AccommodationToRenovateName {  get; set; }

    public string BeginingDate { get; set; }
    public string EndingDate { get; set; }
    
    public string WantedBeginingDate { get; set; }
    public string WantedEndingDate { get; set; }
    public string Description { get; set; }
    
    public int Length { get; set; }

    public RenovationModelViewModel(int id, int accommodationToRenovateId, string accommodationToRenovateName, string beginingDate, string endingDate, string wantedBeginingDate, string wantedEndingDate, string description, int length)
    {
        Id = id;
        AccommodationToRenovateId = accommodationToRenovateId;
        AccommodationToRenovateName = accommodationToRenovateName;
        BeginingDate = beginingDate;
        EndingDate = endingDate;
        WantedBeginingDate = wantedBeginingDate;
        WantedEndingDate = wantedEndingDate;
        Description = description;
        Length = length;
    }
}