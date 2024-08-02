using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;
using BookingApp.WPF.View.TouristPages;

namespace BookingApp.WPF.ViewModel.Tourist;

public class MyRequestsRegularTourViewModel : ViewModelBase
{
    private User _loggedUser;
    private ITourRequestService _tourRequestService;
    private IRequestedTourService _requestedTourService;
    private IUserService _userService;
    public ObservableCollection<RegularTourRequestModelViewModel> RegularTours { get; set; }
    private TouristWindow _touristWindow;
    public ICommand StatisticPageCommand { get; private set; }
 
    
    public MyRequestsRegularTourViewModel(TouristWindow touristWindow)
    {
        var app = Application.Current as App;
        _loggedUser = app.LoggedUser; 
        _tourRequestService = Injector.Container.Resolve<ITourRequestService>();
        _requestedTourService = Injector.Container.Resolve<IRequestedTourService>();
        _userService = Injector.Container.Resolve<IUserService>();
        _touristWindow = touristWindow;
        
        InitializeRegularTours();
        
        StatisticPageCommand = new DelegateCommand(StatisticPageCommandAction);


    }

    private void StatisticPageCommandAction(object param)
    {
        _touristWindow.MainFrame.NavigationService.Navigate(new StatisticPage()
        {
            DataContext = new StatisticViewModel(_touristWindow) 
            {
            }
        });
    }
    private void InitializeRegularTours()
    {
        RegularTours = new ObservableCollection<RegularTourRequestModelViewModel>();
        
        foreach (var tour in _tourRequestService.GetAllByTouristId(_loggedUser.Id))
        {
            string tourGuideName;
            if (tour.Status == TourRequestsStatus.Pending || tour.Status == TourRequestsStatus.Invalid)
            {
                tourGuideName = "Not assigned yet";
            }
            else
            {
                tourGuideName = _userService.GetUsernameById(_requestedTourService.GetByTourRequestId(tour.Id).TourGuideId);
            }
            
            if ((tour.BeginDate - DateTime.Now).TotalHours <= 48 && tour.Status == TourRequestsStatus.Pending)
            {
                tour.Status = TourRequestsStatus.Invalid;
                _tourRequestService.Update(tour);
                tourGuideName = "Not assigned yet";
            }
            
            RegularTours.Add(new RegularTourRequestModelViewModel(
                        tour.Location.Country.ToString() + ", " + tour.Location.City.ToString(),
                        tour.BeginDate,
                        tour.EndDate,
                        tour.Status.ToString(),
                        tourGuideName
                    )
            );
        }
    }
}