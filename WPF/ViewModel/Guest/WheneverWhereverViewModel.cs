using System.Threading;
using BookingApp.WPF.View;

namespace BookingApp.WPF.ViewModel.Guest;

public class WheneverWhereverViewModel : ViewModelBase
{
    private GuestWindow _guestWindow;
    public WheneverWhereverViewModel(GuestWindow guestWindow)
    {
        _guestWindow = guestWindow;
    }
    
}