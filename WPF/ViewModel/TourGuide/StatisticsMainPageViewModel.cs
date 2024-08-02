using System.Windows.Input;
using BookingApp.WPF.View;
using BookingApp.WPF.View.TourGuidePages;

namespace BookingApp.WPF.ViewModel.TourGuide;

public class StatisticsMainPageViewModel
{
    private TourGuideWindow _tourGuideWindow;
    public StatisticsMainPageViewModel(TourGuideWindow tourGuideWindow)
    {
        _tourGuideWindow = tourGuideWindow;
        OpenTourStatisticsPageCommand = new DelegateCommand(OpenTourStatisticsPage);
        OpenTourRequestStatisticsPageCommand = new View.OwnerPages.DelegateCommand(OpenTourRequestStatisticsPage);
    }

    public ICommand OpenTourStatisticsPageCommand { get; private set;}
    private void OpenTourStatisticsPage(object o)
    {
        _tourGuideWindow.MainFrameTourGuideWindow.NavigationService.Navigate(new TourStatisticsPage
        {
            DataContext = new TourStatisticsPageViewModel(_tourGuideWindow)
        });
        
    }
    
    public ICommand OpenTourRequestStatisticsPageCommand { get; private set;}
    private void OpenTourRequestStatisticsPage(object o)
    {
        _tourGuideWindow.MainFrameTourGuideWindow.NavigationService.Navigate(new TourRequestStatisticsPage()
        {
            DataContext = new TourReqestStatisticsViewModel(_tourGuideWindow)
        });
        
    }
}