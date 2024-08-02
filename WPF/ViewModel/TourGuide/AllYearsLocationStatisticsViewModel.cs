using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Utils;
using BookingApp.WPF.View;
using LiveCharts;
using LiveCharts.Wpf;

namespace BookingApp.WPF.ViewModel.TourGuide;

public class AllYearsLocationStatisticsViewModel : ViewModelBase
{
    private TourGuideWindow _tourGuideWindow;
    private string _currentLocation;

    public string CurrentLocation
    {
        get => _currentLocation;
        set
        {
            if (value != _currentLocation)
            {
                _currentLocation = value;
                OnPropertyChanged();
            }
        }
    }


    private ITourRequestService _tourRequestService;

    public AllYearsLocationStatisticsViewModel(TourGuideWindow tourGuideWindow, string location)
    {
        _tourGuideWindow = tourGuideWindow;
        _currentLocation = location;
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
        
        foreach (var request in _tourRequestService.GetByLocation(_currentLocation))
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
    
    public SeriesCollection LocationSeries { get; set; }
    public List<string> Years { get; set; }
    public Func<double, string> IntLabelFormatter { get; } = value => value.ToString("N0");

    private void InitializeStatisticsData()
    {
        Years = new List<string>();
        var requests = _tourRequestService.GetByLocation(_currentLocation);
        var groupedRequests = requests.GroupBy(r => r.TourRequestDate.Year)
            .ToDictionary(g => g.Key, g => g.Count());

        LocationSeries = new SeriesCollection();
        var lineSeries = new LineSeries
        {
            Title = _currentLocation,
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

        LocationSeries.Add(lineSeries);

        OnPropertyChanged(nameof(LocationSeries));
        OnPropertyChanged(nameof(Years));
    }
    
}