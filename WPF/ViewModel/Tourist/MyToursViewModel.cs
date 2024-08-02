using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Dto;
using BookingApp.Utils;
using BookingApp.WPF.View;
using BookingApp.WPF.View.TouristPages;

namespace BookingApp.WPF.ViewModel.Tourist;

public class MyToursViewModel : ViewModelBase
{
    private List<PersonOnTour> _peopleOnTour;
    private ITourReservationService _tourReservationService;
    private IUserService _userService;
    private ITourReviewService _tourReviewService;
    private ITourService _tourService;
    private User _loggedUser;
    private string _currentTourName;
    private string _activeKeypoint;
    private string _touristName;
    private string _arrived;
    private List<TourDto> _finishedTours;
    private DispatcherTimer messageTimer;
    
    private TouristWindow _touristWindow;
    
    private DispatcherTimer successMessageTimer;

    public MyToursViewModel(TouristWindow touristWindow)
    {
        _touristWindow = touristWindow;
        InitializeServices();
        LoadTouristData();
        ConfigureTimers();
        SetupCommands();
    }

    private void InitializeServices()
    {
        var app = Application.Current as App;
        _loggedUser = app.LoggedUser;
        _tourReviewService = Injector.Container.Resolve<ITourReviewService>();
        _tourReservationService = Injector.Container.Resolve<ITourReservationService>();
        _tourService = Injector.Container.Resolve<ITourService>();
        _userService = Injector.Container.Resolve<IUserService>();
    }

    private void LoadTouristData()
    {
        _peopleOnTour = _tourReservationService.GetPeopleOnTour(_loggedUser.Id).ToList();
        var activeTour = _tourService.GetActiveTour(_loggedUser.Id);
        _currentTourName = activeTour != null ? activeTour.Name : "No active tour";
        UpdateTourVisibility(_currentTourName);
        
        _activeKeypoint = _tourService.GetCurrentTourSpot(_loggedUser.Id);
        _touristName = _userService.GetUsernameById(_loggedUser.Id);
        _arrived = _tourReservationService.TouristIsArrived(_loggedUser.Id);
        _finishedTours = _tourService.GetAllFinishedTours(_loggedUser.Id);

        if (RatingSubmissionSuccess)
        {
            RatingSubmissionSuccessVisibility = Visibility.Visible;
            successMessageTimer.Start();
        }
    }

    private void ConfigureTimers()
    {
        successMessageTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(4) };
        successMessageTimer.Tick += SuccessMessageTimer_Tick;

        messageTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(4) };
        messageTimer.Tick += MessageTimer_Tick;
    }

    public ICommand GenerateReportCommand { get; private set; }
    private void SetupCommands()
    {
        RateCommand = new DelegateCommand(RateAction);
        GenerateReportCommand = new DelegateCommand(ExecuteGenerateReport);
    }
    //REPORT
    private void ExecuteGenerateReport(object parameter)
    {
        var tourId = (int)parameter;
        // Logika za generisanje izveštaja za odabrani turu
    }

    private void SuccessMessageTimer_Tick(object sender, EventArgs e)
    {
        RatingSubmissionSuccessVisibility = Visibility.Collapsed;
        successMessageTimer.Stop();
    }

   
    private Visibility _ratingsuccesVisibility = Visibility.Collapsed;
    public Visibility RatingSubmissionSuccessVisibility
    {
        get => _ratingsuccesVisibility;
        set
        {
            if (_ratingsuccesVisibility != value)
            {
                _ratingsuccesVisibility = value;
                OnPropertyChanged(nameof(RatingSubmissionSuccessVisibility)); 
            }
        }
    }
    private bool _ratingSubmissionSuccess;
    public bool RatingSubmissionSuccess
    {
        get => _ratingSubmissionSuccess;
        set
        {
            if (_ratingSubmissionSuccess != value)
            {
                _ratingSubmissionSuccess = value;
                OnPropertyChanged(nameof(RatingSubmissionSuccess));
                RatingSubmissionSuccessVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            
                if (value)
                    successMessageTimer.Start(); // Start the timer only when the flag is set to true
                else
                    successMessageTimer.Stop();  // Stop the timer when the flag is set to false
            }
        }
    }

    
    
    
    private void UpdateTourVisibility(string currentTourName)
    {
        if (currentTourName == "No active tour")
        {
            ActiveTourDetailsVisibility = Visibility.Collapsed;
            NoActiveToursVisibility = Visibility.Visible;
            PeopleOnTourVisibility = Visibility.Collapsed;
        }
        else
        {
            ActiveTourDetailsVisibility = Visibility.Visible;
            NoActiveToursVisibility = Visibility.Collapsed;
            PeopleOnTourVisibility = Visibility.Visible;
        }
    }
    
    public ICommand RateCommand { get; private set; }
    private void MessageTimer_Tick(object sender, EventArgs e)
    {
        RatingMessageVisibility = Visibility.Collapsed;
        messageTimer.Stop();
    }
  
    public List<PersonOnTour> AllPeopleOnTour
    {
        get { return _peopleOnTour; }
        set
        {
            if (value != _peopleOnTour)
            {
                _peopleOnTour = value;
                OnPropertyChanged(nameof(AllPeopleOnTour));
            }
        }
    }
    
    public string CurrentTourName
    {
        get { return _currentTourName; }
        set
        {
            if (value != _currentTourName)
            {
                _currentTourName = value;
                OnPropertyChanged(nameof(CurrentTourName));
            }
        }
    }
    
    public string ActiveKeypoint
    {
        get { return _activeKeypoint; }
        set
        {
            if (value != _activeKeypoint)
            {
                _activeKeypoint = value;
                OnPropertyChanged(nameof(ActiveKeypoint));
            }
        }
    }
    public string TouristName
    {
        get { return _touristName; }
        set
        {
            if (value != _touristName)
            {
                _touristName = value;
                OnPropertyChanged(nameof(TouristName));
            }
        }
    }
    
    public List<TourDto> AllFinishedTours
    {
        get { return _finishedTours; }
        set
        {
            if (value != _finishedTours)
            {
                _finishedTours = value;
                OnPropertyChanged(nameof(AllFinishedTours));
            }
        }
    }
    
    public string Arrived
    {
        get { return _arrived; }
        set
        {
            if (value != _arrived)
            {
                _arrived = value;
                OnPropertyChanged(nameof(Arrived));
            }
        }
    }
    private void RateAction(object param)
    {
        int tourId = (int)param;

        
        if (tourId != null)
        {
            var hasBeenRated = _tourReviewService.HasTourBeenRated(_loggedUser.Id, tourId);

            if (!hasBeenRated)
            {
                
              _touristWindow.MainFrame.NavigationService.Navigate(new RateTourPage
              {
                  DataContext = new RateTourViewModel(_touristWindow, tourId)
              });
            }
            else
            {
               
                RatingMessage = "You have already rated this tour.";
                RatingMessageVisibility = Visibility.Visible;

                messageTimer.Start();
            }
        }
    }
    
    private string _ratingMessage;
    public string RatingMessage
    {
        get => _ratingMessage;
        set
        {
            if (_ratingMessage != value)
            {
                _ratingMessage = value;
                OnPropertyChanged(nameof(RatingMessage));
            }
        }
    }

    private Visibility _ratingMessageVisibility = Visibility.Collapsed;
    public Visibility RatingMessageVisibility
    {
        get => _ratingMessageVisibility;
        set
        {
            if (_ratingMessageVisibility != value)
            {
                _ratingMessageVisibility = value;
                OnPropertyChanged(nameof(RatingMessageVisibility));
            }
        }
    }
    
    
    
    private Visibility _activeTourDetailsVisibility = Visibility.Collapsed;
    public Visibility ActiveTourDetailsVisibility
    {
        get => _activeTourDetailsVisibility;
        set
        {
            _activeTourDetailsVisibility = value;
            OnPropertyChanged(nameof(ActiveTourDetailsVisibility));
        }
    }

    private Visibility _noActiveToursVisibility = Visibility.Visible;
    public Visibility NoActiveToursVisibility
    {
        get => _noActiveToursVisibility;
        set
        {
            _noActiveToursVisibility = value;
            OnPropertyChanged(nameof(NoActiveToursVisibility));
        }
    }

    private Visibility _peopleOnTourVisibility = Visibility.Collapsed;
    public Visibility PeopleOnTourVisibility
    {
        get => _peopleOnTourVisibility;
        set
        {
            _peopleOnTourVisibility = value;
            OnPropertyChanged(nameof(PeopleOnTourVisibility));
        }
    }

}