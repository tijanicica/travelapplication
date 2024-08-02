using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Service;
using BookingApp.Utils;
using BookingApp.WPF.View;
using BookingApp.WPF.View.OwnerPages;

namespace BookingApp.WPF.ViewModel.Owner;

public class OwnerNotificationPageViewModel : ViewModelBase
    {
    private OwnerWindow _ownerWindow;
    private readonly IForumService _forumService;
    private readonly IOwnerNotificationService _notificationsOwnerService;
    public ObservableCollection<AccommodationReservation> notificationsAboutRatingGuests { get; set; }
    public ObservableCollection<Forum> notificationsAboutNewForums { get; set; }

    private User _loggedUser;
    private IUserService _userService;
    public ICommand BackCommand { get; set; }
    public ICommand OpenForumCommand { get; set; }
    public ICommand RateAGuestCommand { get; set; }


    public OwnerNotificationPageViewModel(OwnerWindow ownerWindow, User loggedUser)
    {
        _ownerWindow = ownerWindow;
        var app = Application.Current as App;
        _forumService = Injector.Container.Resolve<IForumService>();
        _notificationsOwnerService = Injector.Container.Resolve<IOwnerNotificationService>();
        _userService = Injector.Container.Resolve<IUserService>();
        _loggedUser = loggedUser;
      
    

      //  notificationsAboutRatingGuests = new ObservableCollection<AccommodationReservation>(_notificationsOwnerService.ChechkRateAGuestNotifications(_loggedUser));
        notificationsAboutNewForums = new ObservableCollection<Forum>(_forumService.GetUnseenForums(_loggedUser.Id)); 

        RateAGuestCommand = new ExecuteCommand<object>(OpenGuestRatingForm);
        BackCommand = new ExecuteCommand<object>(BackMethod);
        OpenForumCommand = new ExecuteCommand<object>(OpenForumMethod);
       
        InitializeForums();
        
    }
    private void InitializeForums()
    {
      
    }
    
    public void OpenForumMethod(object parameter)
    {
        var forum = parameter as Forum;
        forum.HasOwnerSeenTheNotification = true;
        _forumService.Update(forum);
        _ownerWindow.MainFrameOwnerWindow.Content = new SelectedForumPageViewModel(_ownerWindow, _loggedUser, forum, 1);
       // frameMain.Navigate(new SelectedForum(_loggedUser, forum, frameMain, 1));
    }

    public void BackMethod(object parameter)
    {
       // _ownerWindow.MainFrameOwnerWindow.Content = new MyAccommodations(_loggedUser);
        //var page = new MyAccommodations(_loggedUser, frameMain);
        //frameMain.Navigate(page);
    }

    public void OpenGuestRatingForm(object parameter)
    {//ovde nesto ne prebacuje gde treba
       // var reservation = parameter as AccommodationReservation;
     // _ownerWindow.MainFrameOwnerWindow.Content = new GuestReviewPage(_loggedUser, reservation);
        //var page = new GuestReviewPage(_loggedUser, reservation, frameMain);
       // frameMain.Navigate(page);
    }
}