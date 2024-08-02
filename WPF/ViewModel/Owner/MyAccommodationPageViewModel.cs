using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;
using BookingApp.WPF.View.OwnerPages;
using BookingApp.WPF.ViewModel.Tourist;
using DelegateCommand = BookingApp.WPF.View.DelegateCommand;

namespace BookingApp.WPF.ViewModel.Owner;

public class MyAccommodationPageViewModel: ViewModelBase
{
    private User _loggedUser;
    private OwnerWindow _ownerWindow;
    private IAccommodationService _accommodationService;
    public ObservableCollection<MyAccommodationModelViewModel> MyAccommodation { get; set; }
    
    public MyAccommodationPageViewModel(OwnerWindow ownerWindow)
    {
        var app = Application.Current as App;
        _ownerWindow = ownerWindow;
        _loggedUser = app.LoggedUser; 
        _accommodationService = Injector.Container.Resolve<IAccommodationService>();
        InitializeAccommodation();
        
        StatisticsCommand = new DelegateCommand(StatisticsAction);
        RenovationCommand = new DelegateCommand(RenovationAction);


    }
    public ICommand StatisticsCommand { get; private set; }
    public ICommand RenovationCommand { get; private set; }
    private void StatisticsAction(object param)
    {
       
        int accommodationId = (int)param;

        if (accommodationId != null)
        {
            Accommodation accommodation = _accommodationService.GetAccommodationById(accommodationId);
          
            _ownerWindow.MainFrameOwnerWindow.Content = new StatisticsYearPage()
            {
                DataContext = new StatisticsYearPageViewModel(_ownerWindow, accommodation)
            };
        }
    }

 

    private void RenovationAction(object param)
    {
      
        int accommodationId = (int)param;

        if (accommodationId != null)
        {
            Accommodation accommodation = _accommodationService.GetAccommodationById(accommodationId);
          
            _ownerWindow.MainFrameOwnerWindow.Content = new RenovationPage()
            {
                DataContext = new RenovationPageViewModel(_ownerWindow, accommodation)
            };
        }
    }

    private void InitializeAccommodation()
    {
        MyAccommodation = new ObservableCollection<MyAccommodationModelViewModel>();
        
        foreach (var tour in _accommodationService.GetAllByOwnersID(_loggedUser.Id))
        {
      
            MyAccommodation.Add(new MyAccommodationModelViewModel(
                    tour.Id,
                    tour.Name,
                    tour.Location.Country.ToString() + ", " + tour.Location.City.ToString(),
                    tour.Type.ToString()
                )
            );
        }
    }
    
}