using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class ChangeReservationDatesViewModel : ViewModelBase
    {
        private AccommodationReservation _selectedReservation;
        private IAccommodationService _accommodationService;
        private IAccommodationReservationService _accommodationReservationService;
        private IReservationRescheduleService _reservationRescheduleService;
        private readonly GuestWindow _guestWindow;

        public DateTime StartDate => SelectedReservation?.StartDate ?? DateTime.MinValue;

        public DateTime EndDate => SelectedReservation?.EndDate ?? DateTime.MinValue;

        public int Duration => SelectedReservation?.Duration ?? 0;
       
        private MyReservationsViewModel _myReservationsViewModel;

        public ChangeReservationDatesViewModel(AccommodationReservation selectedReservation, MyReservationsViewModel myReservationsViewModel, GuestWindow guestWindow)
        {
            _selectedReservation = selectedReservation;
            SelectedReservation = selectedReservation;
            _accommodationService = Injector.Container.Resolve<IAccommodationService>();
            _accommodationReservationService = Injector.Container.Resolve<IAccommodationReservationService>();
            _reservationRescheduleService = Injector.Container.Resolve<IReservationRescheduleService>();
            SearchCommand = new DelegateCommand(Search);
            _guestWindow = guestWindow;
            _myReservationsViewModel = myReservationsViewModel;
        }


        public AccommodationReservation SelectedReservation
        {
            get { return _selectedReservation; }
            set
            {
                _selectedReservation = value;
                OnPropertyChanged(nameof(SelectedReservation));
                OnPropertyChanged(nameof(AccommodationName)); 
            }
        }

        public string AccommodationName => _accommodationService.GetAccommodationName(SelectedReservation.AccommodationId); 
        private DateTime _newStartDate = DateTime.Today; 
        public DateTime NewStartDate
        {
            get { return _newStartDate; }
            set
            {
                _newStartDate = value;
                OnPropertyChanged(nameof(NewStartDate));
            }
        }

        private DateTime _newEndDate = DateTime.Today; 
        public DateTime NewEndDate
        {
            get { return _newEndDate; }
            set
            {
                _newEndDate = value;
                OnPropertyChanged(nameof(NewEndDate));
            }
        }



        public ICommand SearchCommand { get; private set; }
        

        private List<DateTime> _availableDates;
        public List<DateTime> AvailableDates
        {
            get { return _availableDates; }
            set
            {
                _availableDates = value;
                OnPropertyChanged(nameof(AvailableDates));
            }
        }
        


private void Search(object parameter)
{
    if ((NewEndDate - NewStartDate).Days != Duration)
    {
        ShowWarningMessage($"Please select a duration of {Duration} days.", "Duration Mismatch");
        return;
    }

    List<DateTime> availableDates = _accommodationReservationService.GetAvailableDates(SelectedReservation.AccommodationId, NewStartDate, NewEndDate, Duration);

    if (availableDates.Any())
    {
        ReservationReschedule newReschedule = CreateReservationReschedule(SelectedReservation, availableDates.First(), availableDates.Last());
        _reservationRescheduleService.Save(newReschedule);
        ShowSuccessMessage("Reservation reschedule with available dates has been successfully created and marked as pending.");
    }
    else
    {
        ReservationReschedule newReschedule = CreateReservationReschedule(SelectedReservation, NewStartDate, NewEndDate);
        _reservationRescheduleService.Save(newReschedule);
        ShowWarningMessage("There are no available dates for the selected accommodation.", "No Available Dates");
    }
    UpdateReservationDates(availableDates.First(), availableDates.Last());
    _myReservationsViewModel.UpdateReservations("Current reservations");
    _guestWindow.MainFrameGuestWindow.NavigationService?.GoBack();

}

private ReservationReschedule CreateReservationReschedule(AccommodationReservation selectedReservation, DateTime newStartDate, DateTime newEndDate)
{
    Accommodation accommodation = _accommodationService.GetAccommodationById(selectedReservation.AccommodationId);
    Console.WriteLine($"New Start Date: {newStartDate}, New End Date: {newEndDate}");
    return new ReservationReschedule(
        guestId: selectedReservation.GuestId,
        reservationId: selectedReservation.Id,
        accommodationId: selectedReservation.AccommodationId,
        newStartDate: newStartDate,
        newEndDate: newEndDate,
        reschedulingAnswerStatus: ReschedulingStatus.Pending,
        rejectionComment: "",
        ownerId: accommodation.OwnerId
    );
}

private void UpdateReservationDates(DateTime newStartDate, DateTime newEndDate)
{
    if (SelectedReservation != null)
    {
        Console.WriteLine($"Old Start Date: {SelectedReservation.StartDate}, Old End Date: {SelectedReservation.EndDate}");
        SelectedReservation.StartDate = newStartDate;
        SelectedReservation.EndDate = newEndDate;
        Console.WriteLine($"Updated Start Date: {SelectedReservation.StartDate}, Updated End Date: {SelectedReservation.EndDate}");
    }
}



private void ShowSuccessMessage(string message)
{
    MessageBox.Show(message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
}

private void ShowWarningMessage(string message, string caption)
{
    MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Warning);
}


    }

    internal class ReservationRescheduling
    {
    }
}

