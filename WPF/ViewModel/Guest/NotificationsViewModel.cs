using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Mime;
using System.Windows;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class NotificationsViewModel : ViewModelBase
    {
        private GuestWindow _guestWindow;
        private IGuestNotificationService _guestNotificationService;
        private IUserService _userService;
        private IReservationRescheduleService _reservationRescheduleService;
        private bool _isWindowOpened = false;
        private int Counter = 0;


        private ObservableCollection<GuestNotification> _notifications { get; set; } =
            new ObservableCollection<GuestNotification>();

        private ObservableCollection<GuestNotification> _oldNotifications { get; set; } =
            new ObservableCollection<GuestNotification>();

        private ObservableCollection<GuestNotification> _newNotifications { get; set; } =
            new ObservableCollection<GuestNotification>();
        private User _loggedUser;
        private string _username;
        private string _answer;
        private DateTime _newDate;

        public ObservableCollection<GuestNotification> Notifications
        {
            get { return _notifications; }
            set
            {
                _notifications = value;
                OnPropertyChanged(nameof(Notifications));
            }
        }

        public ObservableCollection<GuestNotification> OldNotifications
        {
            get { return _oldNotifications; }
            set
            {
                _oldNotifications = value;
                OnPropertyChanged(nameof(OldNotifications));
            }
        }
        public ObservableCollection<GuestNotification> NewNotifications
        {
            get { return _newNotifications; }
            set
            {
                _newNotifications = value;
                OnPropertyChanged(nameof(NewNotifications));
            }
        }

        public string Username
        {
            get => _username;
            set { _username = value; }
        }

        public string Answer
        {
            get => _answer;
            set { _answer = value; }
        }

        public DateTime NewDate
        {
            get { return _newDate; }
            set
            {
                _newDate = value;
                OnPropertyChanged(nameof(NewDate));
            }
        }

        public NotificationsViewModel(GuestWindow guestWindow)
        {
            _guestWindow = guestWindow;
            _guestNotificationService = Injector.Container.Resolve<IGuestNotificationService>();
            _userService = Injector.Container.Resolve<IUserService>();
            _reservationRescheduleService = Injector.Container.Resolve<IReservationRescheduleService>();
            LoadNotificationsAndMarkAsRead();
            _isWindowOpened = true;

        }

        private void LoadNotifications()
        {
            var app = Application.Current as App;
            _loggedUser = app.LoggedUser;

            Notifications = new ObservableCollection<GuestNotification>(_guestNotificationService.GetAllByGuestId(_loggedUser.Id));

            foreach (var notification in Notifications)
            {
                if (notification.ReservationId != 0)
                {
                    var reservationReschedule = _reservationRescheduleService.GetByReservationId(notification.ReservationId);
                    if (reservationReschedule != null)
                    {
                        notification.Answer = $"Reservation modification for {reservationReschedule.NewStartDate.ToString("dd/MM/yyyy")} {(reservationReschedule.ReschedulingAnswerStatus == ReschedulingStatus.Accepted ? "has" : "has not")} been approved.";
                    }
                }
                User user = _userService.GetById(notification.OwnerId); // Use UserService to get user
                _username = user.Username; // Set Username property within GuestNotification (optional)
              

                if (notification.IsNotified)
                {
                    _oldNotifications.Add(notification);
                }
                else
                {
                    _newNotifications.Add(notification);
                }
            }
        }

        public void MarkNotificationsAsRead()
        {
            foreach (var notification in _newNotifications)
            {
                notification.IsNotified = true;
                _oldNotifications.Add(notification);
            }
            _newNotifications.Clear();
        }


        public void LoadNotificationsAndMarkAsRead()
        {

            LoadNotifications();

            if(_isWindowOpened)
            {
                // Izdvajanje samo novih obaveštenja
                var newNotifications = _newNotifications.ToList();

                // Označavanje novih obaveštenja kao pročitanih
                foreach (var notification in newNotifications)
                {
                    notification.IsNotified = true;
                    _newNotifications.Remove(notification);
                    _oldNotifications.Add(notification);
                }

            }
        }


       
    }
}
