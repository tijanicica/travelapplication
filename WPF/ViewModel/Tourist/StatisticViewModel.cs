using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;
using LiveCharts;
using LiveCharts.Wpf;

namespace BookingApp.WPF.ViewModel.Tourist;

public class StatisticViewModel : ViewModelBase
{
    private User _loggedUser;
    private ITourRequestService _tourRequestService;
    private TouristWindow _touristWindow;
   
    private string _selectedYear; // Change type to string for straightforward comparison
    public string[]? Years { get; set; } = new[] { "All years", "2022", "2023", "2024" };
    
    public string SelectedYear
    {
        get { return _selectedYear; }
        set
        {
            if (_selectedYear != value)
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
                UpdateStatistics();
            }
        }
    }

    // Pie chart series collection
    public SeriesCollection PieSeries { get; set; }
    public Func<ChartPoint, string> PointLabel { get; set; }
    public Visibility PieChartVisibility { get; set; }
    public Visibility MessageVisibility { get; set; }
    public string Message { get; set; }
    
    //avg people
    public string AverageDisplay { get; set; }


    public StatisticViewModel(TouristWindow touristWindow)
    {
        _touristWindow = touristWindow;
        var app = Application.Current as App;
        _loggedUser = app.LoggedUser;
        _tourRequestService = Injector.Container.Resolve<ITourRequestService>();
        
        SelectedYear = "All years"; // Set default selection here
        PointLabel = chartPoint => 
            $"{((PieSeries)chartPoint.SeriesView).Title}: {chartPoint.Participation:P1}";
        
        PieChartVisibility = Visibility.Visible;
        MessageVisibility = Visibility.Collapsed;
        Message = "";
       
        InitializeChartData();
        UpdateLanguageStatistics(); 
        InitializeLocationStatistics();
        
    }

    private void InitializeChartData()
    {
        PieSeries = new SeriesCollection();
        UpdateStatistics();
    }

    private void UpdateStatistics()
    {
        Application.Current.Dispatcher.Invoke(() => {
            try
            {
                UpdateAverageNumberOfPeople(); 
                var acceptedPercentage = CalculateAcceptedRequestsPercentage();
                var rejectedPercentage = 100 - acceptedPercentage;

                if (acceptedPercentage == 0.0 && rejectedPercentage == 100.0) // Assuming both being zero means no data
                {
                    PieChartVisibility = Visibility.Collapsed;
                    MessageVisibility = Visibility.Visible;
                    Message = "No available data.";
                }
                else
                {
                    PieChartVisibility = Visibility.Visible;
                    MessageVisibility = Visibility.Collapsed;

                    PieSeries.Clear();
                    PieSeries.Add(new PieSeries
                    {
                        Title = "Accepted",
                        Values = new ChartValues<double> { acceptedPercentage },
                        DataLabels = true,
                        LabelPoint = PointLabel,
                        FontSize = 16,
                        FontWeight = FontWeights.Bold
                    });
                    PieSeries.Add(new PieSeries
                    {
                        Title = "Rejected",
                        Values = new ChartValues<double> { rejectedPercentage },
                        DataLabels = true,
                        LabelPoint = PointLabel,
                        FontSize = 16,
                        FontWeight = FontWeights.Bold
                    });
                }

                OnPropertyChanged(nameof(PieSeries));
                OnPropertyChanged(nameof(PieChartVisibility));
                OnPropertyChanged(nameof(MessageVisibility));
                OnPropertyChanged(nameof(Message));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating statistics: " + ex.ToString());
            }
        });
    }

    private double CalculateAcceptedRequestsPercentage()
    {
        return _tourRequestService.GetAcceptedRequestsPercentage(_loggedUser.Id, SelectedYear);
    }
    private void UpdateAverageNumberOfPeople()
    {
        double average = _tourRequestService.GetAveragePeopleInAcceptedRequests(_loggedUser.Id, SelectedYear);
        AverageDisplay = $"{average:N1}"; 
        OnPropertyChanged(nameof(AverageDisplay));
    }
    
    //language
    public SeriesCollection LanguageSeries { get; set; }

    public List<string> LanguageLabels { get; set; } // Stores the language labels for the chart

    private void UpdateLanguageStatistics()
    {
        var languageStats = _tourRequestService.GetRequestCountsByLanguage();

        LanguageSeries = new SeriesCollection();
        LanguageLabels = new List<string>(); // Make sure this is initialized before populating
        foreach (var stat in languageStats)
        {
            LanguageSeries.Add(new ColumnSeries
            {
                Title = stat.Language,
                Values = new ChartValues<int> { stat.Count },
                MaxColumnWidth = 50, // Sets maximum width of bars
            });
            LanguageLabels.Add(stat.Language); // Correctly populating the language labels
        }

        OnPropertyChanged(nameof(LanguageSeries));
        OnPropertyChanged(nameof(LanguageLabels));
    }


    public Func<double, string> YAxisLabelFormatter => value => Math.Round(value).ToString();
    


    // Location chart data
    public SeriesCollection LocationSeries { get; set; }
    public List<string> LocationLabels { get; set; } // Stores the location labels for the chart

    private void InitializeLocationStatistics()
    {
        var locationStats = _tourRequestService.GetRequestCountsByLocation();

        LocationSeries = new SeriesCollection();
        LocationLabels = new List<string>();
        foreach (var stat in locationStats)
        {
            LocationSeries.Add(new ColumnSeries
            {
                Title = stat.LocationName,
                Values = new ChartValues<int> { stat.Count }
            });
            LocationLabels.Add(stat.LocationName);
        }

        OnPropertyChanged(nameof(LocationSeries));
        OnPropertyChanged(nameof(LocationLabels));
    }
}
