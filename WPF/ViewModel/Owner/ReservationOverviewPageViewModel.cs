using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Dto;
using BookingApp.Utils;
using BookingApp.WPF.View;

namespace BookingApp.WPF.ViewModel.Owner;

public class ReservationOverviewPageViewModel: ViewModelBase
{
        private OwnerWindow _ownerWindow;
        private readonly IReservationRescheduleService _reservationRescheduleService;
        private readonly IAccommodationReservationService _accommodationReservationService;
        private readonly IGuestNotificationService _guestNotificationService;
        private User _loggedUser;
        private List<ReservationRescheduleDto> _reservationRescheduleDtoList;
        public ICommand ApproveCommand { get; private set; }
        public ICommand RejectCommand { get; private set; }
        


        public ReservationOverviewPageViewModel(OwnerWindow ownerWindow)
        {
            _ownerWindow = ownerWindow;
            var app = Application.Current as App;
            _reservationRescheduleService = Injector.Container.Resolve<IReservationRescheduleService>();
            _accommodationReservationService = Injector.Container.Resolve<IAccommodationReservationService>();
            _guestNotificationService = Injector.Container.Resolve<IGuestNotificationService>();
            _reservationRescheduleDtoList = new List<ReservationRescheduleDto>();
            
            _loggedUser = app.LoggedUser;
            LoadData();
            ApproveCommand = new DelegateCommand(Approve);
            RejectCommand = new DelegateCommand(Reject);
        }

   
        private void LoadData()
        {
            ReservationRescheduleDtoList = _reservationRescheduleService.GetAllByOwnerId(_loggedUser.Id).ToList();
        }

        public List<ReservationRescheduleDto> ReservationRescheduleDtoList
        {
            get => _reservationRescheduleDtoList;
            set
            {
                _reservationRescheduleDtoList = value;
                OnPropertyChanged(nameof(ReservationRescheduleDtoList));
            }
        }

   

        private void Approve(object parameter)
        {
            int reservationId = (int)parameter;

            if (reservationId != null)
            {
                ReservationReschedule reservationReschedule =
                    _reservationRescheduleService.GetById(reservationId);
                reservationReschedule.ReschedulingAnswerStatus = ReschedulingStatus.Accepted;
                AccommodationReservation accommodationReservation =
                    _accommodationReservationService.GetById(reservationReschedule.ReservationId);
                accommodationReservation.StartDate = reservationReschedule.NewStartDate;
                accommodationReservation.EndDate = reservationReschedule.NewEndDate;
                _accommodationReservationService.Update(accommodationReservation);

                _reservationRescheduleService.Update(reservationReschedule);
                var app = System.Windows.Application.Current as App;
                GuestNotification notification = new GuestNotification()
                {
      
                    ReservationId = reservationId,
                    NewDate = DateTime.Now, 
                    GuestId = accommodationReservation.GuestId,
                    OwnerId = app.LoggedUser.Id,
                    Username = _loggedUser.Username,
                    Answer = $"Reservation modification for {reservationReschedule.NewStartDate.ToString("dd/MM/yyyy")} {(reservationReschedule.ReschedulingAnswerStatus == ReschedulingStatus.Accepted ? "has" : "has not")} been approved."

                };
                _guestNotificationService.Save(notification);
                MessageBox.Show($"Reservation approved!");
             
                
            }
        }

        private void Reject(object parameter)
        {
            int reservationId = (int)parameter;

            if (reservationId != null)
            {
                string comment = Microsoft.VisualBasic.Interaction.InputBox("Unesite komentar:", "Komentar", "");


                if (!string.IsNullOrEmpty(comment))
                {
                    ReservationReschedule reservationReschedule =
                        _reservationRescheduleService.GetById(reservationId);
                    reservationReschedule.ReschedulingAnswerStatus = ReschedulingStatus.Rejected;
                    reservationReschedule.RejectionComment = comment;
                    _reservationRescheduleService.Update(reservationReschedule);
                    AccommodationReservation accommodationReservation = 
                        _accommodationReservationService.GetById(reservationReschedule.ReservationId);
                    var app = System.Windows.Application.Current as App;
                    GuestNotification notification = new GuestNotification()
                    {
      
                        ReservationId = reservationId,
                        NewDate = DateTime.Now, 
                        GuestId = accommodationReservation.GuestId,
                        OwnerId = app.LoggedUser.Id,
                        Username = _loggedUser.Username,
                        Answer = $"Reservation modification for {reservationReschedule.NewStartDate.ToString("dd/MM/yyyy")} {(reservationReschedule.ReschedulingAnswerStatus == ReschedulingStatus.Accepted ? "has" : "has not")} been approved."

                    };
                    _guestNotificationService.Save(notification);

                }
            }
        }

    }
    