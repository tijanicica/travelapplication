using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Dto;
using BookingApp.Utils;
using BookingApp.WPF.ViewModel.TourGuide;

namespace BookingApp.WPF.View.TourGuidePages;

public partial class TrackOneTourPage : Page, INotifyPropertyChanged
{

    private TourGuideWindow _tourGuideWindow;

    private ITourReservationService _tourReservationService;
    private ITourService _tourService;
    private ITourExecutionService _tourExecutionService;
    private IUserService _userService;
    private INotificationService _notificationService;
    
    /*private TourReservationController _tourReservationController;
    private TourController _tourController;
    private TourExectuionController _tourExecutionController;
    private UserController _userController;*/
    private TourDto _currentTour;
    private List<TourSpot> _tourSpots;
    //private NotificationController _notificationController;


    private string _touristName;

    public string TouristName
    {
        get => _touristName;
        set
        {
            if (value != _touristName)
            {
                _touristName = value;
                OnPropertyChanged();
            }
        }
    }

    private ObservableCollection<TourSpot> _currentTourTourSpots;

    public ObservableCollection<TourSpot> CurrentTourTourSpots
    {
        get => _currentTourTourSpots;
        set
        {
            if (value != _currentTourTourSpots)
            {
                _currentTourTourSpots = value;
                OnPropertyChanged();
            }
        }
    }

    private ObservableCollection<JoinedTouristViewModel> _currentTourTourists;

    public ObservableCollection<JoinedTouristViewModel> CurrentTourTourists
    {
        get => _currentTourTourists;
        set
        {
            if (value != _currentTourTourists)
            {
                _currentTourTourists = value;
                OnPropertyChanged();
            }
        }
    }

    private ObservableCollection<PersonOnTour> _peopleOnTour;

    public ObservableCollection<PersonOnTour> OtherPeopleOnTour
    {
        get => _peopleOnTour;
        set
        {
            if (value != _peopleOnTour)
            {
                _peopleOnTour = value;
                OnPropertyChanged(nameof(OtherPeopleOnTour));
            }
        }
    }

    private string _currentTourName;
    public string CurrentTourName { get { return _currentTour?.Name; } 
        set { } }
    private string _tourSpotDescription;
    private User _loggedUser;

    public TrackOneTourPage(TourGuideWindow tourGuideWindow, TourDto currentTour)
    {
        InitializeComponent();
        this.DataContext = this;
        var app = Application.Current as App;
        /*_tourController = app.TourController;
        _tourExecutionController = app.TourExectuionController;
        _userController = app.UserController;
        _tourReservationController = app.TourReservationController;*/
        _tourService = Injector.Container.Resolve<ITourService>();
        _tourExecutionService = Injector.Container.Resolve<ITourExecutionService>();
        _userService = Injector.Container.Resolve<IUserService>();
        _notificationService = Injector.Container.Resolve<INotificationService>();
        _tourReservationService = Injector.Container.Resolve<ITourReservationService>();

        _tourGuideWindow = tourGuideWindow;
        _currentTour = currentTour;
        _loggedUser = app.LoggedUser;
       // _notificationController = app.NotificationController;
        _currentTourName = CurrentTourName;
        _tourSpots = _tourService.GetTourSpotsByTourId(_tourService.GetTourIdByExecutionId(currentTour.Id))
            .ToList();
        CurrentTourTourSpots =
            new ObservableCollection<TourSpot>(_tourSpots);
        InitializeTourists();
        InitializeOthersOnTour();


    }

    private void InitializeOthersOnTour()
    {
        OtherPeopleOnTour = new ObservableCollection<PersonOnTour>();
        List<TourReservation> reservations = _tourReservationService.GetByTourExecutionId(_currentTour.Id).ToList();
        foreach (var reservation in reservations)
        {
            foreach (var person in reservation.OtherPeopleOnTour)
            {
                OtherPeopleOnTour.Add(person);
            }
        }

        _peopleOnTour = OtherPeopleOnTour;

    }

    private void InitializeTourists()
    {
        CurrentTourTourists =
            new ObservableCollection<JoinedTouristViewModel>();
        foreach (var tourist in _tourExecutionService.GetTourTouristsById(_currentTour.Id))
        {
            string tourSpotDescription = "";
            if (tourist.JoinedAtTourSpot != -1)
            {

                tourSpotDescription = _tourSpots.Find(spot => spot.Id == tourist.JoinedAtTourSpot).Description;
            }

            CurrentTourTourists.Add(new JoinedTouristViewModel(_userService.GetUsernameById(tourist.TouristId),
                tourSpotDescription));


        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void CheckBox_Checked(object sender, RoutedEventArgs e)
    {
        CheckBox checkBox = sender as CheckBox;
        if (checkBox != null)
        {
            TourSpot tourSpot = checkBox.DataContext as TourSpot;
            if (tourSpot != null)
            {
                tourSpot.Visited = true;
                if (tourSpot.End)
                {
                    EndCurrentTour();
                }
            }

            Tour tour = _tourService.GetById(_tourService.GetTourIdByExecutionId(_currentTour.Id));
            tour.TourSpots.Find(t => t.Id == tourSpot.Id).Visited = true; //promeni u memoriji
            TourExecution tourExecution = _tourExecutionService.GetById(_currentTour.Id);
            tourExecution.CurrentTourSpotId =
                tour.TourSpots.Find(t => t.Id == tourSpot.Id).Id;
            _tourExecutionService.Update(tourExecution);
            _tourService.Update(tour);
        }

    }

    private TourSpot _selectedTourSpot;

    public TourSpot? SelectedTourSpot
    {
        get => _selectedTourSpot;
        set
        {
            if (value != _selectedTourSpot)
            {
                _selectedTourSpot = value;
                OnPropertyChanged();
            }
        }
    }

  

    private void JoinTourist(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        if (button == null) return;

        var selectedTourist = button.DataContext as JoinedTouristViewModel;
        if (selectedTourist == null) return;

        var existingTourist = CurrentTourTourists.FirstOrDefault(t => t.TouristName == selectedTourist.TouristName);
        if (existingTourist == null || SelectedTourSpot == null || !SelectedTourSpot.Visited) return;

        existingTourist.JoinedTourSpot = SelectedTourSpot.Description;

        foreach (var tourist in _tourExecutionService.GetTourTouristsById(_currentTour.Id))
        {
            if (_userService.GetUsernameById(tourist.TouristId) == selectedTourist.TouristName)
            {
                tourist.JoinedAtTourSpot = SelectedTourSpot.Id;
                UpdateTouristInMemory(tourist);
            }
        }

        CurrentTourTourists = new ObservableCollection<JoinedTouristViewModel>(CurrentTourTourists);
    }

    private void UpdateTouristInMemory(TourTourist tourist)
    {
        var execution = _tourExecutionService.GetById(_currentTour.Id);
        var touristInExecution = execution.Tourists.Find(t => t.TouristId == tourist.TouristId);
        if (touristInExecution != null)
        {
            touristInExecution.JoinedAtTourSpot = SelectedTourSpot.Id;
        }

        _tourExecutionService.Update(execution);
    }
    
    private void EndTour(object sender, RoutedEventArgs e)
    {
        EndCurrentTour();
    }

    private void EndCurrentTour()
    {
        
        TourExecution execution = _tourExecutionService.GetById(_currentTour.Id);
        execution.Status = Status.Finished;
        _tourExecutionService.Update(execution);

        RestartTourSpots();
        
        SendNotification();

        MessageBox.Show("Your tour has ended.");

        if (_tourGuideWindow != null && _tourGuideWindow.MainFrameTourGuideWindow != null)
        {
            _tourGuideWindow.MainFrameTourGuideWindow.Navigate(new HomePage(_tourGuideWindow));
        }
        
        
    }

    private void RestartTourSpots()
    {
        Tour tour = _tourService.GetById(_tourService.GetTourIdByExecutionId(_currentTour.Id));
        foreach (var tourSpot in tour.TourSpots)
        {
            tourSpot.Visited = false;
        }

        _tourService.Update(tour);
    }

    private void SendNotification()
    {
        
        foreach (var tourist in _tourExecutionService.GetTourTouristsById(_currentTour.Id))
        {
                Notification notification = new Notification();
                notification.TouristId = tourist.TouristId;
                notification.SentDateTime = DateTime.Now;
                notification.IsAccepted = false;
                notification.IsSeen = false;
                notification.TourGuideId = _loggedUser.Id;
                notification.TourExecutionId = _currentTour.Id;

                notification.MessageContent =  _tourReservationService.GetArrivedPeopleOnTourByTouristId(tourist.TouristId, _currentTour.Id);

                _notificationService.Save(notification);
           
        }

    }
    
    
    private void ArrivedCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        CheckBox checkBox = sender as CheckBox;
        if (checkBox != null)
        {
            PersonOnTour person = checkBox.DataContext as PersonOnTour;
            if (person != null)
            {
                MarkPersonAsArrived(person);
                UpdateCurrentReservationByPerson(person);
            }
        }
    }

    private void MarkPersonAsArrived(PersonOnTour person)
    {
        if (person != null)
        {
            person.Arrived = true;
        }
    }

    private void UpdateCurrentReservationByPerson(PersonOnTour person)
    {
        if (person != null)
        {
            List<TourReservation> reservations =
                _tourReservationService.GetByTourExecutionId(_currentTour.Id).ToList();
            foreach (var reservation in reservations)
            {
                UpdateReservationByPerson(reservation, person);
            }
        }
    }

    private void UpdateReservationByPerson(TourReservation reservation, PersonOnTour person)
    {
        if (reservation != null && reservation.OtherPeopleOnTour != null)
        {
            var personOnTour = reservation.OtherPeopleOnTour.Find(p => p.Id == person.Id);
            if (personOnTour != null)
            {
                personOnTour.Arrived = true;
                _tourReservationService.Update(reservation);
            }
        }
    }


}

