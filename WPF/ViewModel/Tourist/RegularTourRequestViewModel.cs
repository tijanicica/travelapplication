using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;
using BookingApp.WPF.View.TouristPages;

namespace BookingApp.WPF.ViewModel.Tourist;

public class RegularTourRequestViewModel : ViewModelBase
{
    private User _loggedUser;
    private ITourRequestService _tourRequestService;
    public ObservableCollection<PersonOnTourViewModel> PeopleOnTour { get; set; }
    public ICommand SubmitTourRequestCommand { get; private set; }
    public ICommand CancelTourRequestCommand { get; private set; }
    private TouristWindow _touristWindow;
    public string[]? Languages { get; set; } = new[] { "English", "Serbian", "Spanish", "French" };
    public int GuidesLanguage { get; set; } = -1;


    public RegularTourRequestViewModel(TouristWindow touristWindow)
    {
        var app = Application.Current as App;
        _loggedUser = app.LoggedUser; 
        _tourRequestService = Injector.Container.Resolve<ITourRequestService>();
        _touristWindow = touristWindow;
        
        PeopleOnTour = new ObservableCollection<PersonOnTourViewModel>();
        PeopleOnTour.CollectionChanged += PeopleOnTour_CollectionChanged;
        
        SubmitTourRequestCommand = new DelegateCommand(SubmitTourRequest);
        CancelTourRequestCommand = new DelegateCommand(CancelTourRequest);
        
    }
    private void PeopleOnTour_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems != null)
        {
            foreach (PersonOnTourViewModel item in e.OldItems)
            {
                item.PropertyChanged -= PersonOnTour_PropertyChanged;
            }
        }
    
        if (e.NewItems != null)
        {
            foreach (PersonOnTourViewModel item in e.NewItems)
            {
                item.PropertyChanged += PersonOnTour_PropertyChanged;
            }
        }
    }
    private void PersonOnTour_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        // Update IsValid whenever any property of any PersonOnTourViewModel changes
        OnPropertyChanged(nameof(IsValid));
    }
    //polja
    private Location _location = new Location();
    
    public Location Location
    {
        get => _location;
        set
        {
            _location = value;
            OnPropertyChanged(nameof(Location));
            OnPropertyChanged(nameof(IsValid));
        }
    }

    public string Country
    {
        get => _location.Country;
        set
        {
            _location.Country = value;
            OnPropertyChanged(nameof(Country));
            OnPropertyChanged(nameof(IsValid));
        }
    }

    public string City
    {
        get => _location.City;
        set
        {
            _location.City = value;
            OnPropertyChanged(nameof(City));
            OnPropertyChanged(nameof(IsValid));
        }
    }
    
    private string _description;
    
  
    private DateTime _endDate = DateTime.Today.AddDays(2);

    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            OnPropertyChanged(nameof(Description));
            OnPropertyChanged(nameof(IsValid));
        }
    }

   

    private DateTime _beginDate = DateTime.Today.AddDays(2); // Initialize to today's date

    public DateTime BeginDate
    {
        get => _beginDate;
        set
        {
            if (_beginDate != value)
            {
                _beginDate = value;
                OnPropertyChanged(nameof(BeginDate));
                OnPropertyChanged(nameof(IsValid));
            }
        }
    }


    public DateTime EndDate
    {
        get => _endDate;
        set
        {
            _endDate = value;
            OnPropertyChanged(nameof(EndDate));
            OnPropertyChanged(nameof(IsValid));
        }
    }


    private void CancelTourRequest(object parameter)
    {
        HomePage homePage = new HomePage();

        // Navigira se do HomePage
        _touristWindow.MainFrame.NavigationService.Navigate(homePage);
    }


    private void SubmitTourRequest(object parameter)
    {
        TourRequest tourRequest = new TourRequest();

        tourRequest.PeopleOnTour = PeopleOnTour.Select(vm => new PersonOnTour
        {
            Name = vm.Name,
            Surename = vm.Surname,
            Age = vm.Age
        }).ToList();
        tourRequest.TourRequestDate = DateTime.Now;
        tourRequest.TouristId = _loggedUser.Id;
        Enum.TryParse<Language>(Languages[GuidesLanguage], out var type);
        tourRequest.Language = type;
        tourRequest.Description = this.Description;
        tourRequest.Location = this.Location; 
        tourRequest.BeginDate = this.BeginDate;
        tourRequest.EndDate = this.EndDate;
        tourRequest.Status = TourRequestsStatus.Pending;



        _tourRequestService.Save(tourRequest);
        
        // Navigate to HomePage and pass the message
    Application.Current.Dispatcher.Invoke(() =>
    {
        HomePage homePage = new HomePage
        {
            MessageText = "Successful tour request!",
            MessageVisibility = Visibility.Visible
        };

        _touristWindow.MainFrame.NavigationService.Navigate(homePage);
    });
    }
    
    //dinamicki
    private int _touristCount;
    public int TouristCount
    {
        get => _touristCount;
        set
        {
            if (_touristCount != value)
            {
                _touristCount = value;
                GenerateTouristInputs(_touristCount);
                OnPropertyChanged(nameof(TouristCount));
                OnPropertyChanged(nameof(IsValid));
            }
        }
    }


    private void GenerateTouristInputs(int count)
    {
        PeopleOnTour.Clear();
        for (int i = 0; i < count; i++)
        {
            PeopleOnTour.Add(new PersonOnTourViewModel()); // Changed from PersonOnTour to PersonOnTourViewModel
        }
    }
    
    private bool ValidateFields()
    {
        if (string.IsNullOrWhiteSpace(Country) || string.IsNullOrWhiteSpace(City) ||
            string.IsNullOrWhiteSpace(Description) || GuidesLanguage == -1 || 
            PeopleOnTour.Any(p => string.IsNullOrWhiteSpace(p.Name) || 
                                  string.IsNullOrWhiteSpace(p.Surname) ||
                                  p.Age <= 0))
        {
            return false;
        }

        if (TouristCount <= 0 || BeginDate <= DateTime.Today || EndDate <= BeginDate)
        {
            return false;
        }

        return true;
    }
    
    public bool IsValid
    {
        get { return ValidateFields(); }
    }
    
    
    
}

