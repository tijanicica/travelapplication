using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;

namespace BookingApp.WPF.View.TourGuidePages;

public partial class ScheduleTourPage : Page, INotifyPropertyChanged
{
    private TourGuideWindow _tourGuideWindow = null;
    private string _errorMessage;
    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            if (value != _errorMessage)
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }
    }
    private DispatcherTimer _errorTimer;
    
    private ObservableCollection<TourExecution> _tourExecutions;

    public ObservableCollection<TourExecution> TourExecutions
    {
        get => _tourExecutions;
        set
        {
            if (value != _tourExecutions)
            {
                _tourExecutions = value;
                OnPropertyChanged();
            }
        }
    }

    private ObservableCollection<Tour> _allTours;

    public ObservableCollection<Tour> AllTours
    {
        get => _allTours;
        set
        {
            if (value != _allTours)
            {
                _allTours = value;
                OnPropertyChanged();
            }
        }
    }

    public int? Hours { get; set; }
    private Tour _selectedTour;

    public Tour? SelectedTour
    {
        get => _selectedTour;
        set
        {
            if (value != _selectedTour)
            {
                _selectedTour = value;
                TourExecutions =
                    new ObservableCollection<TourExecution>(_tourExecutionService.GetTourExecutions(_selectedTour.Id));
                OnPropertyChanged();
            }
        }
    }

    public DateTime? StartDate { get; set; }
    //private TourController _tourController;
    private ITourService _tourService;
    private ITourExecutionService _tourExecutionService;
    private User _loggedUser;

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


    public ScheduleTourPage(TourGuideWindow tourGuideWindow)
    {
        InitializeComponent();

        this.DataContext = this;
        var app = Application.Current as App;
        _tourGuideWindow = tourGuideWindow;
        //_tourController = app.TourController;
        _tourService = Injector.Container.Resolve<ITourService>();
        _tourExecutionService = Injector.Container.Resolve<ITourExecutionService>();
        _loggedUser = app.LoggedUser;

        AllTours = new ObservableCollection<Tour>(_tourService.GetByTourGuideId(_loggedUser.Id));
        TourExecutions = new ObservableCollection<TourExecution>();
        _errorTimer = new DispatcherTimer();
        _errorTimer.Interval = TimeSpan.FromSeconds(5);
        _errorTimer.Tick += ClearErrorMessage;
    }
    
    private bool ValidateFields()
    {
        if ( Hours < 1 || Hours > 23 || Hours == null )
            
        {
            ShowErrorMessage ( "Error: You must enter valid number between 1 and 23 for starting hours.");
            return false;
        }
        if (StartDate <= DateTime.Today )
            
        {
            ShowErrorMessage ( "Error: Tour scheduling is only available for dates in the future.");
            return false;
        }
        if (StartDate == null )
            
        {
            ShowErrorMessage ( "Error: You must enter a start date for the tour.");
            return false;
        }
        if (SelectedTour == null)
            
        {
            ShowErrorMessage ( "Error: You must select a tour you want to schedule.");
            return false;
        }
        
        return true;
    }
    
    private void ShowErrorMessage(string message)
    {
        ErrorMessage = message;
        _errorTimer.Start(); 
    }

    private void ClearErrorMessage(object sender, EventArgs e)
    {
        ErrorMessage = "";
        _errorTimer.Stop(); 
    }
    
    private bool CanScheduleTour(DateTime proposedStartDate, double duration)
    {
        DateTime endDateTime = proposedStartDate.AddHours(duration);
        foreach (var tour in _tourService.GetByTourGuideId(_loggedUser.Id))
        {
            foreach (var execution in _tourExecutionService.GetTourExecutions(tour.Id))
            {
                DateTime executionEndDate = execution.StartDate.AddHours(duration);
                if (proposedStartDate <= executionEndDate && execution.StartDate <= endDateTime) 
                {
                    return false; 
                }
            }
        }
        
        return true; 
    }

    
  
    private void ScheduleTour(object sender, RoutedEventArgs e)
    {
        if (ValidateFields())
        {
            DateTime proposedStartDate = new DateTime(StartDate.Value.Year, StartDate.Value.Month, StartDate.Value.Day, Hours.Value, 0, 0);
            
            if (!CanScheduleTour(proposedStartDate, SelectedTour.Duration))
            {
                MessageBox.Show("You can schedule this tour again when the previous one is finished.");
                return;
            }
            

            TourExecution tourExecution = new TourExecution(-1, SelectedTour.Id, Status.Inactive, -1, new List<TourTourist>(),
                proposedStartDate);
            _tourExecutionService.Save(tourExecution);
            TourExecutions =
                new ObservableCollection<TourExecution>(_tourExecutionService.GetTourExecutions(_selectedTour.Id));
            MessageBox.Show("Tour added to the schedule list");
        }
        
    }


    private void DoneButtonClick(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Tour successfully scheduled!");
        
        if (_tourGuideWindow != null && _tourGuideWindow.MainFrameTourGuideWindow != null)
        {
            _tourGuideWindow.MainFrameTourGuideWindow.Navigate(new HomePage(_tourGuideWindow));
        }
    }
}