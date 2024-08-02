using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;
using BookingApp.WPF.View.TourGuidePages;

namespace BookingApp.WPF.ViewModel.TourGuide;

public class AllTourRequestsPageViewModel : ViewModelBase
{
    private TourGuideWindow _tourGuideWindow;
  
   
    
    
    private ObservableCollection<AllTourRequestsModelViewModel> _requests;
    public ObservableCollection<AllTourRequestsModelViewModel> Requests
    {
        get => _requests;
        set
        {
            if (value != _requests)
            {
                _requests = value;
                OnPropertyChanged();
            }
        }
    }
    
    private ObservableCollection<AllTourRequestsModelViewModel> _allRequests;
    public ObservableCollection<AllTourRequestsModelViewModel> AllRequests
    {
        get => _allRequests;
        set
        {
            if (value != _allRequests)
            {
                _allRequests = value;
                OnPropertyChanged();
            }
        }
    }

    private string _countryFilter;
    public string CountryFilter
    {
        get => _countryFilter;
        set
        {
            if (value != _countryFilter)
            {
                _countryFilter = value;
                OnPropertyChanged();
                FilterRequests();

            }
        }
    }
    
    private string _cityFilter;
    public string CityFilter
    {
        get => _cityFilter;
        set
        {
            if (value != _cityFilter)
            {
                _cityFilter = value;
                OnPropertyChanged();
                FilterRequests();

            }
        }
    }
    private string _languageFilter;
    public string LanguageFilter
    {
        get => _languageFilter;
        set
        {
            if (value != _languageFilter)
            {
                _languageFilter = value;
                OnPropertyChanged();
                FilterRequests();

            }
        }
    }
    private string _noPeopleFilter;
    public string NoPeopleFilter
    {
        get => _noPeopleFilter;
        set
        {
            if (value != _noPeopleFilter)
            {
                _noPeopleFilter = value;
                OnPropertyChanged();
                FilterRequests();

            }
        }
    }
    private DateTime _dateToday = DateTime.Today;
    public DateTime DateToday
    {
        get => _dateToday;
        set
        {
            if (value != _dateToday)
            {
                _dateToday = value;
                OnPropertyChanged();

            }
        }
    }
    
    private DateTime? _startDateFilter;
    public DateTime? StartDateFilter
    {
        get => _startDateFilter;
        set
        {
            if (value != _startDateFilter)
            {
                _startDateFilter = value;
                OnPropertyChanged();
                FilterRequests();

            }
        }
    }

    private DateTime? _endDateFilter;
    public DateTime? EndDateFilter
    {
        get => _endDateFilter;
        set
        {
            if (value != _endDateFilter)
            {
                _endDateFilter = value;
                OnPropertyChanged();
                FilterRequests();
            }
        }
    }
    
    
    

    private ITourRequestService _tourRequestService;

    public ICommand OpenOneTourRequestPageCommand { get; private set; }
    public ICommand AceeptTourRequestCommand { get; private set; }

    public AllTourRequestsPageViewModel(TourGuideWindow tourGuideWindow)
    {
        _tourGuideWindow = tourGuideWindow;
        _tourRequestService = Injector.Container.Resolve<ITourRequestService>();
        OpenOneTourRequestPageCommand = new DelegateCommand(OpenOneTourRequestPage);
        AceeptTourRequestCommand = new DelegateCommand(AcceptTourRequest);
        InitializeRequests();
        Requests = new ObservableCollection<AllTourRequestsModelViewModel>(AllRequests);
    }

    private void InitializeRequests()
    {
        AllRequests = new ObservableCollection<AllTourRequestsModelViewModel>();
        foreach (var request in _tourRequestService.GetAll())
        {
            int noPeople = request.PeopleOnTour.Count + 1;
            AllRequests.Add(new AllTourRequestsModelViewModel(
                request.Id,
                request.Location.Country,
                request.Location.City,
                request.Language.ToString(),
                request.BeginDate.Date.ToShortDateString(),
                request.EndDate.Date.ToShortDateString(),
                request.Status.ToString(),
                noPeople.ToString()
            ));
        }
    }



    private void FilterRequests()
    {
        Requests = new ObservableCollection<AllTourRequestsModelViewModel>(AllRequests);
        if (!String.IsNullOrEmpty(CountryFilter))
        {
            Requests = new ObservableCollection<AllTourRequestsModelViewModel>(Requests.Where(r =>
                r.Country.Contains(CountryFilter)));
        }
        if (!String.IsNullOrEmpty(CityFilter))
        {
            Requests = new ObservableCollection<AllTourRequestsModelViewModel>(Requests.Where(r =>
                r.City.Contains(CityFilter)));
        }
        if (!String.IsNullOrEmpty(LanguageFilter))
        {
            Requests = new ObservableCollection<AllTourRequestsModelViewModel>(Requests.Where(r =>
                r.Language.Contains(LanguageFilter)));
        }
        if (!String.IsNullOrEmpty(NoPeopleFilter))
        {
            Requests = new ObservableCollection<AllTourRequestsModelViewModel>(Requests.Where(r =>
                r.NoPeople == NoPeopleFilter
        ));
        }
        if (StartDateFilter != null && EndDateFilter != null && EndDateFilter > StartDateFilter)
        {
            Requests = new ObservableCollection<AllTourRequestsModelViewModel>(Requests.Where(r =>
                DateTime.Parse(r.BeginDate) >= StartDateFilter && DateTime.Parse(r.EndDate) <= EndDateFilter));
        }
        
    }

    private void OpenOneTourRequestPage(object param)
    {
        
        // Button viewMoreButton = sender as Button;
        //TourReviewsViewModel selectedReview = viewMoreButton?.DataContext as TourReviewsViewModel;
        int requestId = (int)param;
        if (requestId != null)
        {
            TourRequest currentRequest = _tourRequestService.GetById(requestId);
            _tourGuideWindow.MainFrameTourGuideWindow.NavigationService.Navigate(new OneTourRequestPage
            {
                DataContext = new OneTourRequestPageViewModel(_tourGuideWindow,
                    currentRequest)
            });
            
        }
    }

    private void AcceptTourRequest(object param)
    {
        int requestId = (int)param;
        if (requestId != null)
        {
            TourRequest currentRequest = _tourRequestService.GetById(requestId);
            if (currentRequest.Status == TourRequestsStatus.Pending)
            {
                _tourGuideWindow.MainFrameTourGuideWindow.NavigationService.Navigate(new PickDateForTourRequestPage
                {
                    DataContext = new PickDateForTourRequestViewModel(_tourGuideWindow,
                        currentRequest)
                });
            }
            
            
        }
    }
}