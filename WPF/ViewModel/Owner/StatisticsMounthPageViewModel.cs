using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;
using BookingApp.WPF.View.OwnerPages;

namespace BookingApp.WPF.ViewModel.Owner;

public class StatisticsMounthPageViewModel: ViewModelBase
{
    private OwnerWindow _ownerWindow;
    public IAccommodationReservationService _reservationService {  get; set; }
    public IAccommodationService _accommodationService { get; set; }
    private StatisticsAccommodation _currentStat;
   
    private User _loggedUser;
    public ObservableCollection<StatisticsAccommodation> statsForSelectedAccommodationByMonth { get; set; }
    public string Name { get; set; }
    public int year { get; set; }
   // public ICommand BackCommand { get; set; }

    public StatisticsMounthPageViewModel(OwnerWindow ownerWindow, StatisticsAccommodation currentStat)
    {
        _ownerWindow = ownerWindow;
        var app = Application.Current as App;
        _loggedUser = app.LoggedUser; 
        _currentStat = currentStat;
        
        _accommodationService = Injector.Container.Resolve<IAccommodationService>();
        _reservationService = Injector.Container.Resolve<IAccommodationReservationService>();
      //  Accommodation  accommodation = _accommodationService.GetAccommodationById(_currentStat.AccommodationId);
        
        Initialize();
        
        //BackCommand = new ExecuteCommand<object>(BackMethod);
        
    }
    
    private void Initialize()
    {
        Name = _currentStat.AccommodationName;
        year = _currentStat.Year;
        
        statsForSelectedAccommodationByMonth = _reservationService.GetStatsPerMonth(_currentStat);
        
        bussiestMonth =
            _reservationService.GetBussiestMonth(statsForSelectedAccommodationByMonth);
        
    }
   
    
    public int bussiestMonth { get; set; }
    
    
}