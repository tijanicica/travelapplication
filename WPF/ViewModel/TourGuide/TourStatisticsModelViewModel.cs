namespace BookingApp.WPF.ViewModel.TourGuide;

public class TourStatisticsModelViewModel
{
    public string Name { get; set; }
    public int Id { get; set; }

    public TourStatisticsModelViewModel(string name, int id)
    {
        Name = name;
        Id = id;
    }
    public TourStatisticsModelViewModel(string name)
    {
        Name = name;
       
    }
}