using BookingApp.Domain.Model;

namespace BookingApp.WPF.ViewModel.Owner;
public class SuperOwnerModelViewModel: ViewModelBase
{
    public string Username { get; set; }
    public UserType Type {  get; set; }
    public string superOwner { get; set; }

    public SuperOwnerModelViewModel()
    {
    }

    public SuperOwnerModelViewModel(string username, UserType type, string superOwner)
    {
        Username = username;
        Type = type;
        this.superOwner = superOwner;
    }
}