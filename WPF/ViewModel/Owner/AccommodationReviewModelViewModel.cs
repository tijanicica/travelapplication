namespace BookingApp.WPF.ViewModel.Owner;

public class AccomodationReviewModelViewModel: ViewModelBase
{
    public int ratingID { get; set; } //id same ocene koju gost daje smestaju i vlasniku
    public int guestID { get; set; } //samo se prosledi id ulogovanog gosta
    public int ownerID { get; set; }
    public string ownerName { get; set; }
    public string guestsName { get; set; }
    public int accommodationCleanliness { get; set; }
    public int ownerCorrectness { get; set; }
    public string additionalComment { get; set; }

    public AccomodationReviewModelViewModel()
    {
    }

    public AccomodationReviewModelViewModel(int ratingId, int guestId, int ownerId, string ownerName, string guestsName, int accommodationCleanliness, int ownerCorrectness, string additionalComment)
    {
        ratingID = ratingId;
        guestID = guestId;
        ownerID = ownerId;
        this.ownerName = ownerName;
        this.guestsName = guestsName;
        this.accommodationCleanliness = accommodationCleanliness;
        this.ownerCorrectness = ownerCorrectness;
        this.additionalComment = additionalComment;
    }
}