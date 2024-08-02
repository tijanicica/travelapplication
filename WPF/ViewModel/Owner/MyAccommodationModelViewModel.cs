namespace BookingApp.WPF.ViewModel.Owner;

public class MyAccommodationModelViewModel: ViewModelBase
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public string Type { get; set; }
   

    public MyAccommodationModelViewModel(int id, string name, string location, string type)
    {
        Id = id;
        Name = name;
        Location = location;
        Type = type;
      
    }
}