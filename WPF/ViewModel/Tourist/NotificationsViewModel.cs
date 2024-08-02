using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Dto;
using BookingApp.Utils;
using BookingApp.WPF.View;
using BookingApp.WPF.View.TouristPages;

namespace BookingApp.WPF.ViewModel.Tourist;

public class NotificationsViewModel : ViewModelBase
{
    private List<NotificationsDto> _allNotificationsDtos;        
    private ITourService _tourService;

    private INotificationService _notificationService;
    private IRequestedTourService _requestedTourService;
    private ITourRequestService _tourRequestService;
    private IUserService _userService;
    private User _loggedUser;
    private TouristWindow _touristWindow;
    public ObservableCollection<NotificationRegularTourModelViewModel> RegularTours { get; set; }
    public NotificationsViewModel(TouristWindow touristWindow)
    {
        var app = Application.Current as App;
        _touristWindow = touristWindow;
        _loggedUser = app.LoggedUser; 
        _tourService = Injector.Container.Resolve<ITourService>();
        _notificationService = Injector.Container.Resolve<INotificationService>();
        _requestedTourService = Injector.Container.Resolve<IRequestedTourService>();
        _tourRequestService = Injector.Container.Resolve<ITourRequestService>();
        _userService = Injector.Container.Resolve<IUserService>();
 
        _allNotificationsDtos =  _tourService.GetNotificationDtos(_loggedUser.Id);
        InitializeRegularTours();

     
      DismissCommand = new DelegateCommand(DismissAction);
      ConfirmCommand = new DelegateCommand(ConfirmAction);

    }
    private void InitializeRegularTours()
    {
        RegularTours = new ObservableCollection<NotificationRegularTourModelViewModel>();
        List<TourRequest> tourRequests = _tourRequestService.GetAllByTouristId(_loggedUser.Id).Where(e=>e.Status == TourRequestsStatus.Accepted).ToList();
        List<RequestedTour> requestedTours = _requestedTourService.GetAll();
        List<RequestedTour> requestedToursNotification = new List<RequestedTour>();
        
        foreach (var tourRequest in tourRequests)
        {
            foreach (var requestedTour in requestedTours)
            {
                if (tourRequest.Id == requestedTour.TourRequestId)
                {
                    requestedToursNotification.Add(requestedTour);
                }
            }
            
        }
        
        foreach (var tour in requestedToursNotification)
        {
            RegularTours.Add(new NotificationRegularTourModelViewModel(
                    _tourRequestService.GetById(tour.TourRequestId).Location.Country.ToString() + ", " +
                    _tourRequestService.GetById(tour.TourRequestId).Location.City.ToString(),
                        _userService.GetUsernameById(tour.TourGuideId),
                        tour.Date
                    )
            );
        }
    }
    public ICommand DismissCommand { get; private set; }
    public ICommand ConfirmCommand { get; private set; }
    public List<NotificationsDto> AllNotificationsDtos
    {
        get { return _allNotificationsDtos; }
        set
        {
            if (value != _allNotificationsDtos)
            {
                _allNotificationsDtos = value;
                OnPropertyChanged(nameof(AllNotificationsDtos));
            }
        }
    }
    

    private void ConfirmAction(object param)
    {
       
        int notificationId = (int)param;

        if (notificationId != null)
        {
            Notification notification = _notificationService.GetById(notificationId);
            notification.IsAccepted = true;
            _notificationService.Update(notification);
            
          _touristWindow.MainFrame.Content = new NotificationsPage()
          {
              DataContext = new NotificationsViewModel(_touristWindow)
          };
        }
    }

 

    private void DismissAction(object param)
    {
       int notificationId = (int)param;
       
        if (notificationId != null)
        {
            Notification notification = _notificationService.GetById(notificationId);
            _notificationService.Delete(notification);
            _allNotificationsDtos = _tourService.GetNotificationDtos(_loggedUser.Id);
      
        }
    }
    
}