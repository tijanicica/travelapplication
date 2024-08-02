using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;
using BookingApp.WPF.View.TourGuidePages;

namespace BookingApp.WPF.ViewModel.TourGuide;

public class OneTourPartPageViewModel : ViewModelBase
{
    private readonly TourGuideWindow _tourGuideWindow;
    private readonly ComplexTourPart _currentTourPart;
       private readonly User _loggedUser;

    private readonly ITourService _tourService;
    private readonly ITourExecutionService _tourExecutionService;
    private readonly IRequestedTourService _requestedTourService;
    private readonly IComplexTourPartService _tourPartService;
    private readonly IComplexTourRequestService _complexTourRequestService;
    private readonly ObservableCollection<ComplexTourPart> _allTourParts;
    private string _language;

    public string Language
    {
        get => _language;
        set
        {
            if (value != _language)
            {
                _language = value;
                OnPropertyChanged();
            }
        }
    }

    private string _location;

    public string Location
    {
        get => _location;
        set
        {
            if (value != _location)
            {
                _location = value;
                OnPropertyChanged();
            }
        }
    }

    private string _startDate;

    public string StartDate
    {
        get => _startDate;
        set
        {
            if (value != _startDate)
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }
    }

    private string _endDate;

    public string EndDate
    {
        get => _endDate;
        set
        {
            if (value != _endDate)
            {
                _endDate = value;
                OnPropertyChanged();
            }
        }
    }

    private string _requestDate;

    public string RequestDate
    {
        get => _requestDate;
        set
        {
            if (value != _requestDate)
            {
                _requestDate = value;
                OnPropertyChanged();
            }
        }
    }

    private string _status;

    public string Status
    {
        get => _status;
        set
        {
            if (value != _status)
            {
                _status = value;
                OnPropertyChanged();
            }
        }
    }

    private string _description;

    public string Description
    {
        get => _description;
        set
        {
            if (value != _description)
            {
                _description = value;
                OnPropertyChanged();
            }
        }
    }

    private ObservableCollection<PersonOnTour> _peopleOnTour;

    public ObservableCollection<PersonOnTour> PeopleOnTour
    {
        get => _peopleOnTour;
        set
        {
            if (value != _peopleOnTour)
            {
                _peopleOnTour = value;
                OnPropertyChanged();
            }
        }
    }

    private ObservableCollection<DateTime> _availableDates;

    public ObservableCollection<DateTime> AvailableDates
    {
        get => _availableDates;
        set
        {
            if (value != _availableDates)
            {
                _availableDates = value;
                OnPropertyChanged();
            }
        }
    }
    private bool _canAccept;
    public bool CanAccept
    {
        get => _canAccept;
        set
        {
            if (value != _canAccept)
            {
                _canAccept = value;
                OnPropertyChanged();
            }
        }
    }
    public ICommand AcceptTourPartCommand { get; }


    public OneTourPartPageViewModel(TourGuideWindow tourGuideWindow, ComplexTourPart currentTourPart, ObservableCollection<ComplexTourPart> allTourParts)
    {
        var app = Application.Current as App;
        _loggedUser = app.LoggedUser;
        _tourGuideWindow = tourGuideWindow;
        _currentTourPart = currentTourPart;
        _allTourParts = allTourParts;
        _tourService = Injector.Container.Resolve<ITourService>();
        _tourExecutionService = Injector.Container.Resolve<ITourExecutionService>();
        _requestedTourService = Injector.Container.Resolve<IRequestedTourService>();
        _tourPartService = Injector.Container.Resolve<IComplexTourPartService>();
        _complexTourRequestService = Injector.Container.Resolve<IComplexTourRequestService>();
        AcceptTourPartCommand = new RelayDateCommand<DateTime>(AcceptTourPart, () => CanAccept);
        InitializeTourPart();
        InitializePeople();
        CalculateAvailableDates();
    }
    

    private void InitializeTourPart()
    {
        Language = _currentTourPart.Language.ToString();
        Location = _currentTourPart.Location.City + ", " + _currentTourPart.Location.Country;
        StartDate = _currentTourPart.BeginDate.ToShortDateString();
        EndDate = _currentTourPart.EndDate.ToShortDateString();
        RequestDate = _currentTourPart.TourRequestDate.ToShortDateString();
        Status = _currentTourPart.Status.ToString();
        Description = _currentTourPart.Description;
        CheckAcceptButtonState();

    }

    private void InitializePeople()
    {
        PeopleOnTour = new ObservableCollection<PersonOnTour>();
        foreach (var person in _currentTourPart.PeopleOnTour)
        {
            PersonOnTour personOnTour = new PersonOnTour();
            personOnTour.Name = person.Name;
            personOnTour.Surename = person.Surename;
            personOnTour.Age = person.Age;
            PeopleOnTour.Add(personOnTour);
        }
    }
    private void CheckAcceptButtonState()
    {
        CanAccept = _currentTourPart.Status == ComplexTourPartStatus.Pending &&
                    !IsAnyTourPartAcceptedByGuide() &&
                    GuideIsAvailable(_currentTourPart.BeginDate);
        // Notify the command that the execution state might have changed
        (AcceptTourPartCommand as RelayDateCommand<DateTime>)?.RaiseCanExecuteChanged();
    }

    private bool IsAnyTourPartAcceptedByGuide()
    {
        return _allTourParts.Any(tp => tp.TourGuideId == _loggedUser.Id && tp.Status == ComplexTourPartStatus.Accepted);
    }

    private void CalculateAvailableDates()
    {
        AvailableDates = new ObservableCollection<DateTime>();

        DateTime startDate = _currentTourPart.BeginDate;
        DateTime endDate = _currentTourPart.EndDate;

        for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
        {
            if (IsDateAvailable(date) && GuideIsAvailable(date))
            {
                AvailableDates.Add(date);
            }
        }
    }

    private bool IsDateAvailable(DateTime date)
    {
        return _allTourParts
            .Where(tp => tp.TourGuideId != _currentTourPart.TourGuideId && tp.Status == ComplexTourPartStatus.Accepted)
            .All(tp => date < tp.BeginDate || date > tp.EndDate);
    }

    private bool GuideIsAvailable(DateTime proposedStartDate)
    {
        foreach (var tour in _tourService.GetByTourGuideId(_loggedUser.Id))
        {
            foreach (var execution in _tourExecutionService.GetTourExecutions(tour.Id))
            {
                if (proposedStartDate == execution.StartDate)
                {
                    return false;
                }
            }
        }

        foreach (var requestedTour in _requestedTourService.GetAll())
        {
            if (proposedStartDate == requestedTour.Date)
            {
                return false;
            }
        }

        return true;
    }

    private void AcceptTourPart(DateTime date)
    {
        if (_currentTourPart.Status == ComplexTourPartStatus.Pending)
        {
            if (IsAnyTourPartAcceptedByGuide())
            {
                MessageBox.Show("You have already accepted a tour part.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                CanAccept = false;
            }
            else if (!GuideIsAvailable(date))
            {
                MessageBox.Show("The guide is not available on the selected date.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                _currentTourPart.AcceptedDate = date;
                _currentTourPart.TourGuideId = _loggedUser.Id;
                _currentTourPart.Status = ComplexTourPartStatus.Accepted;
                // update taj tour part
                _tourPartService.Update(_currentTourPart);
                OnPropertyChanged(nameof(Status));
                MessageBox.Show("Tour part accepted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _tourGuideWindow.MainFrameTourGuideWindow.Content = new OneTourPartPage()
                {
                    DataContext = new OneTourPartPageViewModel(_tourGuideWindow, _currentTourPart, _allTourParts)
                };
                CanAccept = false;
            }
        }
    }

}