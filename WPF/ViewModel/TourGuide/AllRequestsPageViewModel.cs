using System.Windows.Input;
using BookingApp.WPF.View;
using BookingApp.WPF.View.TourGuidePages;

namespace BookingApp.WPF.ViewModel.TourGuide;

public class AllRequestsPageViewModel
{
    private TourGuideWindow _tourGuideWindow;

    public AllRequestsPageViewModel(TourGuideWindow tourGuideWindow)
    {
        _tourGuideWindow = tourGuideWindow;
        OpenAllTourRequestsPageCommand = new DelegateCommand(OpenAllTourRequestsPage);
        OpenAllComplexRequestsPageCommand = new DelegateCommand(OpenAllComplexRequestsPage);
    }
    
    public ICommand OpenAllTourRequestsPageCommand { get; private set;}
    public ICommand OpenAllComplexRequestsPageCommand { get; private set; }
    private void OpenAllTourRequestsPage(object o)
    {
        _tourGuideWindow.MainFrameTourGuideWindow.NavigationService.Navigate(new AllTourRequestsPage()
        {
            DataContext = new AllTourRequestsPageViewModel(_tourGuideWindow)
        });
        
    }

    private void OpenAllComplexRequestsPage(object o)
    {
        _tourGuideWindow.MainFrameTourGuideWindow.NavigationService.Navigate(new AllComplexRequestsPage()
        {
            DataContext = new AllComplexRequestsPageViewModel(_tourGuideWindow)
        });
    }
}