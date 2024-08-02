using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Utils;
using BookingApp.WPF.View;
using LiveCharts;
using LiveCharts.Wpf;

namespace BookingApp.WPF.ViewModel.TourGuide;

public class AllYearsLanguageStatisticsViewModel : ViewModelBase
{
    private TourGuideWindow _tourGuideWindow;
    private string _currentLanguage;
    public string CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            if (value != _currentLanguage)
            {
                _currentLanguage = value;
                OnPropertyChanged();
            }
        }
    }


    private ITourRequestService _tourRequestService;

    public AllYearsLanguageStatisticsViewModel(TourGuideWindow tourGuideWindow, string language)
    {
        _tourGuideWindow = tourGuideWindow;
        _currentLanguage = language;
        _tourRequestService = Injector.Container.Resolve<ITourRequestService>();
        _statisticsByYears = new Dictionary<int, int>();
        CalculateStatistics();
        InitializeStatisticsData();

    }

    private Dictionary<int, int> _statisticsByYears;
    public Dictionary<int, int> StatisticsByYears
    {
        get => _statisticsByYears;
        set
        {
            if (value != _statisticsByYears)
            {
                _statisticsByYears = value;
                OnPropertyChanged();
            }
        }
    } 

    private void CalculateStatistics()
    {
        
        foreach (var request in _tourRequestService.GetByLanguage(_currentLanguage))
        {
            if (_statisticsByYears.Keys.Contains(request.TourRequestDate.Year))
            {
                _statisticsByYears[request.TourRequestDate.Year] += 1;
            }
            else
            {
                
                _statisticsByYears[request.TourRequestDate.Year] = 1;
            }
            
        }
    }
    public SeriesCollection LanguageSeries { get; set; }
    public List<string> Years { get; set; }

    private void InitializeStatisticsData()
    {
        Years = new List<string>();
        var requests = _tourRequestService.GetByLanguage(_currentLanguage);
        var groupedRequests = requests.GroupBy(r => r.TourRequestDate.Year)
            .ToDictionary(g => g.Key, g => g.Count());

        LanguageSeries = new SeriesCollection();
        var lineSeries = new LineSeries
        {
            Title = _currentLanguage,
            Values = new ChartValues<int>(),
            PointGeometry = null, 
            LineSmoothness = 0,
            Stroke = Brushes.White
        };

        foreach (var year in groupedRequests.Keys.OrderBy(y => y))
        {
            Years.Add(year.ToString());
            lineSeries.Values.Add(groupedRequests[year]);
        }

        LanguageSeries.Add(lineSeries);

        OnPropertyChanged(nameof(LanguageSeries));
        OnPropertyChanged(nameof(Years));
    }
    
 
}