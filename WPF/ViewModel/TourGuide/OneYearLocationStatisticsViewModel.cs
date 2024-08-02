using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;
using LiveCharts;
using LiveCharts.Wpf;

namespace BookingApp.WPF.ViewModel.TourGuide;

public class OneYearLocationStatisticsViewModel : ViewModelBase
{
    private TourGuideWindow _tourGuideWindow;
    private string _currentLocation;
    private int _currentYear;

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

    public int CurrentYear
    {
        get => _currentYear;
        set
        {
            if (value != _currentYear)
            {
                _currentYear = value;
                OnPropertyChanged();
            }
        }
    }


    private ITourRequestService _tourRequestService;


    public OneYearLocationStatisticsViewModel(TourGuideWindow tourGuideWindow, string currentLocation, int currentYear)
    {
        _tourGuideWindow = tourGuideWindow;
        _currentLocation = currentLocation;
        _currentYear = currentYear;
        _tourRequestService = Injector.Container.Resolve<ITourRequestService>();
        _statisticsByMonths = new Dictionary<int, int>();
        CalculateStatistics();
        InitializeStatisticsData();
    }

    private Dictionary<int, int> _statisticsByMonths;

    public Dictionary<int, int> StatisticsByMonths
    {
        get => _statisticsByMonths;
        set
        {
            if (value != _statisticsByMonths)
            {
                _statisticsByMonths = value;
                OnPropertyChanged();
            }
        }
    }

    private void CalculateStatistics()
    {
        foreach (var request in _tourRequestService.GetByLocationAndYear(_currentLocation, _currentYear))

            if (_statisticsByMonths.Keys.Contains(request.TourRequestDate.Month))
            {
                _statisticsByMonths[request.TourRequestDate.Month] += 1;
            }
            else
            {
                _statisticsByMonths[request.TourRequestDate.Month] = 1;
            }
    }
    
    public SeriesCollection LocationSeries { get; set; }
    public List<string> Months { get; set; }
    public Func<double, string> IntLabelFormatter { get; } = value => value.ToString("N0");

    private void InitializeStatisticsData()
    {
        Months = new List<string>();
        var requests = _tourRequestService.GetByLocation(_currentLocation);
        var groupedRequests = requests.GroupBy(r => r.TourRequestDate.Month)
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
            Months.Add(year.ToString());
            lineSeries.Values.Add(groupedRequests[year]);
        }

        LocationSeries.Add(lineSeries);

        OnPropertyChanged(nameof(LocationSeries));
        OnPropertyChanged(nameof(Months));
    }
}


