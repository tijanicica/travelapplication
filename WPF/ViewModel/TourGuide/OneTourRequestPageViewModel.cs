using System.Collections.ObjectModel;
using BookingApp.Domain.Model;
using BookingApp.WPF.View;

namespace BookingApp.WPF.ViewModel.TourGuide;

public class OneTourRequestPageViewModel : ViewModelBase
{
    private TourGuideWindow _tourGuideWindow;
    private TourRequest _currentRequest;
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
    
    public OneTourRequestPageViewModel(TourGuideWindow tourGuideWindow, TourRequest currentRequest)
    {
        _tourGuideWindow = tourGuideWindow;
        _currentRequest = currentRequest;
        Description = currentRequest.Description;
        InitializeInfo();
        InitializePeople();

    }

    private string _description;

    public string Description
    {
        get => _description;
        set {
            if (value != _description)
            {
                _description = value;
                OnPropertyChanged();
            }}
    }
    private string _startDate;

    public string StartDate
    {
        get => _startDate;
        set {
            if (value != _startDate)
            {
                _startDate = value;
                OnPropertyChanged();
            }}
    }
    
    private string _endDate;

    public string EndDate
    {
        get => _endDate;
        set {
            if (value != _endDate)
            {
                _endDate = value;
                OnPropertyChanged();
            }}
    }
    
    private string _requestDate;

    public string RequestDate
    {
        get => _requestDate;
        set {
            if (value != _requestDate)
            {
                _requestDate = value;
                OnPropertyChanged();
            }}
    }
    
    private string _location;

    public string Location
    {
        get => _location;
        set {
            if (value != _location)
            {
                _location = value;
                OnPropertyChanged();
            }}
    }
    private string _language;

    public string Language
    {
        get => _language;
        set {
            if (value != _language)
            {
                _language = value;
                OnPropertyChanged();
            }}
    }
    
    private string _noPeople;

    public string NoPeople
    {
        get => _noPeople;
        set {
            if (value != _noPeople)
            {
                _noPeople = value;
                OnPropertyChanged();
            }}
    }
    
    private string _status;

    public string Status
    {
        get => _status;
        set {
            if (value != _status)
            {
                _status = value;
                OnPropertyChanged();
            }}
    }
    private string _name;

    public string Name
    {
        get => _name;
        set {
            if (value != _name)
            {
                _name = value;
                OnPropertyChanged();
            }}
    }
    
    private string _surname;

    public string Surname
    {
        get => _surname;
        set {
            if (value != _surname)
            {
                _surname = value;
                OnPropertyChanged();
            }}
    }
    
    private string _age;

    public string Age
    {
        get => _age;
        set {
            if (value != _age)
            {
                _age = value;
                OnPropertyChanged();
            }}
    }
    


    private void InitializeInfo()
    {
        Location = _currentRequest.Location.City.ToString() + ',' + _currentRequest.Location.Country.ToString();
        StartDate = _currentRequest.BeginDate.Date.ToShortDateString();
        EndDate = _currentRequest.EndDate.Date.ToShortDateString();
        RequestDate = _currentRequest.TourRequestDate.Date.ToShortDateString();
        Language = _currentRequest.Language.ToString();
        NoPeople = (_currentRequest.PeopleOnTour.Count + 1).ToString();
        Status = _currentRequest.Status.ToString();
    }

    private void InitializePeople()
    {
        PeopleOnTour = new ObservableCollection<PersonOnTour>();
        foreach (var person in _currentRequest.PeopleOnTour)
        {
            PersonOnTour personOnTour = new PersonOnTour();
            personOnTour.Name = person.Name;
            personOnTour.Surename = person.Surename;
            personOnTour.Age = person.Age;
            PeopleOnTour.Add(personOnTour);
        }
       
    }
    
}