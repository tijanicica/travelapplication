using BookingApp.WPF.View;

namespace BookingApp.WPF.ViewModel.Guest;

public class ForumsViewModel  : ViewModelBase
{
    private GuestWindow _guestWindow;

    public ForumsViewModel(GuestWindow guestWindow)
    {
        _guestWindow = guestWindow;
    }

}