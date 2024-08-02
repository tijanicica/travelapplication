using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Utils;
using BookingApp.WPF.View;
using BookingApp.WPF.View.TourGuidePages;
using DelegateCommand = BookingApp.WPF.View.OwnerPages.DelegateCommand;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace BookingApp.WPF.ViewModel.TourGuide;

public class TourReqestStatisticsViewModel : ViewModelBase
{
    private TourGuideWindow _tourGuideWindow;
    private string _errorMessageLocation;
    public string ErrorMessageLocation
    {
        get => _errorMessageLocation;
        set
        {
            if (value != _errorMessageLocation)
            {
                _errorMessageLocation = value;
                OnPropertyChanged();
            }
        }
    }
    
    private string _errorMessageLanguage;
    public string ErrorMessageLanguage
    {
        get => _errorMessageLanguage;
        set
        {
            if (value != _errorMessageLanguage)
            {
                _errorMessageLanguage = value;
                OnPropertyChanged();
            }
        }
    }
    
    private DispatcherTimer _errorTimer;
    private int _yearLocation;

    public int YearLocation { get; set; } = -1;
    

    private int _yearLanguage;

    public int YearLanguage { get; set; } = -1;
    

    private string _location;

    public int Location { get; set; } = -1;
    
    public string[] Locations { get; set; } = new[] { "Rome", "New York", "Tokyo", "Sydney", "Amsterdam", "Dortmund", "Orlando"};
    

    public int Language { get; set; } = -1;
    

    private string _mostWantedLocation;

    public string MostWantedLocation
    {
        get => _mostWantedLocation;
        set
        {
            if (value != _mostWantedLocation)
            {
                _mostWantedLocation = value;
                OnPropertyChanged();
            }
        }
    }

    private string _mostWantedLanguage;

    public string MostWantedLanguage
    {
        get => _mostWantedLanguage;
        set
        {
            if (value != _mostWantedLanguage)
            {
                _mostWantedLanguage = value;
                OnPropertyChanged();
            }
        }
    }

    private ITourRequestService _tourRequestService;
    
    public string[] Languages { get; set; } = new[] { "English", "Serbian" };

    public int[] Years { get; set; } = new[] { 2022, 2023, 2024 };

    private HashSet<string> _typedLocations;

    public ObservableCollection<string> LocationSuggestions { get; private set; }

    public TourReqestStatisticsViewModel(TourGuideWindow tourGuideWindow)
    {
        _tourGuideWindow = tourGuideWindow;
        OpenAllYearsLocationStatisticsCommand = new DelegateCommand(OpenAllYearsLocationStatistics);
        OpenAllYearsLanguageStatisticsCommand = new DelegateCommand(OpenAllYearsLanguageStatistics);
        OpenOneYearLocationStatisticsCommand = new DelegateCommand(OpenOneYearLocationStatistics);
        OpenOneYearLanguageStatisticsCommand = new DelegateCommand(OpenOneYearLanguageStatistics);
        LocationTextChangedCommand = new ExecuteCommand<string>(LocationTextChanged);
        CreateTourWithLocationCommand = new DelegateCommand(CreateTourWithLocation);
        CreateTourWithLanguageCommand = new DelegateCommand(CreateTourWithLanguage);
        _tourRequestService = Injector.Container.Resolve<ITourRequestService>();
        _typedLocations = new HashSet<string>();
        LocationSuggestions = new ObservableCollection<string>();
        MostWantedLocation = _tourRequestService.GetMostWantedLocation();
        MostWantedLanguage = _tourRequestService.GetMostWantedLanguage();
        _errorTimer = new DispatcherTimer();
        _errorTimer.Interval = TimeSpan.FromSeconds(5);
        _errorTimer.Tick += ClearErrorMessageLocation;
        _errorTimer.Tick += ClearErrorMessageLanguage;
    }

    private bool ValidateFieldsLocation()
    {
        

        if (Location == -1)
        {
            ShowErrorMessageLocation ( "Error: You must select wanted location first.");
            return false;
        }
        if (YearLocation == -1 )
        {
            ShowErrorMessageLocation ( "Error: You must select wanted year first.");
            return false;
        }
        return true;
    }
    
    private bool ValidateFieldsLanguage()
    {
        if ( Language == -1 )
        {
            ShowErrorMessageLanguage ( "Error: You must select wanted language first.");
            return false;
        }
        
        if ( YearLanguage == -1)
        {
            ShowErrorMessageLanguage ( "Error: You must select wanted year first.");
            return false;
        }
        return true;
    }
    private void ShowErrorMessageLocation(string message)
    {
        ErrorMessageLocation = message;
        _errorTimer.Start(); // Start the timer to clear the error message after 5 seconds
    }

    private void ClearErrorMessageLocation(object sender, EventArgs e)
    {
        ErrorMessageLocation = "";
        _errorTimer.Stop(); // Stop the timer
    }
    
    private void ShowErrorMessageLanguage(string message)
    {
        ErrorMessageLocation = message;
        _errorTimer.Start(); // Start the timer to clear the error message after 5 seconds
    }

    private void ClearErrorMessageLanguage(object sender, EventArgs e)
    {
        ErrorMessageLocation = "";
        _errorTimer.Stop(); // Stop the timer
    }

    public ICommand OpenAllYearsLocationStatisticsCommand { get; private set; }
    public ICommand OpenOneYearLocationStatisticsCommand { get; private set; }
    public ICommand OpenOneYearLanguageStatisticsCommand { get; private set; }
    public ICommand CreateTourWithLocationCommand { get; private set; }
    public ICommand CreateTourWithLanguageCommand { get; private set; }
    public ICommand LocationTextChangedCommand { get; private set; }

    private void CreateTourWithLocation(object o)
    {
        _tourGuideWindow.MainFrameTourGuideWindow.Content = new CreateTourPage(_tourGuideWindow, _mostWantedLocation);


    }
    
    private void CreateTourWithLanguage(object o)
    {
        _tourGuideWindow.MainFrameTourGuideWindow.Content =
            new CreateTourPage(_tourGuideWindow, null, _mostWantedLanguage);

    }

    private void OpenOneYearLanguageStatistics(object o)
    {
        if (ValidateFieldsLanguage())
        {
            _tourGuideWindow.MainFrameTourGuideWindow.Content = new OneYearLanguageStatisticsPage()
            {
                DataContext =
                    new OneYearLanguageStatisticsViewModel(_tourGuideWindow, Languages[Language], Years[YearLanguage])
            };
        }
    }

    private void OpenOneYearLocationStatistics(object o)
    {
        if (ValidateFieldsLocation())
        {
            _tourGuideWindow.MainFrameTourGuideWindow.Content = new OneYearLocationStatisticsPage()
            {
                DataContext =
                    new OneYearLocationStatisticsViewModel(_tourGuideWindow, Locations[Location], Years[YearLocation])
            };
        }
    }

    private void OpenAllYearsLocationStatistics(object param)
    {
        
            if (Location != -1)
            {
                _tourGuideWindow.MainFrameTourGuideWindow.Content = new AllYearsLocationStatisticsPage()
                {
                    DataContext =
                        new AllYearsLocationStatisticsViewModel(_tourGuideWindow, Locations[Location])
                };
            }
            else
            {
                MessageBox.Show("Please enter location first.");

            }
       
       
       
        
    }

    public ICommand OpenAllYearsLanguageStatisticsCommand { get; private set; }

    private void OpenAllYearsLanguageStatistics(object param)
    {
        if (Language != -1)
        {
            _tourGuideWindow.MainFrameTourGuideWindow.Content = new AllYearsLanguageStatisticsPage()
            {
                DataContext =
                    new AllYearsLanguageStatisticsViewModel(_tourGuideWindow, Languages[Language])
            };
        }
        else
        {
            MessageBox.Show("Please enter language first.");

        }
        
       
    }
    public void AddTypedLocation(string location)
    {
        _typedLocations.Add(location);
        UpdateLocationSuggestions();
    }

    private void UpdateLocationSuggestions()
    {
        LocationSuggestions = new ObservableCollection<string>(_typedLocations);
        OnPropertyChanged(nameof(LocationSuggestions));
    }

    private void LocationTextChanged(string text)
    {
        AddTypedLocation(text);
    }
    
}