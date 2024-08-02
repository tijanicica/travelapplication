using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;
using BookingApp.WPF.View.TourGuidePages;
using DelegateCommand = BookingApp.WPF.View.OwnerPages.DelegateCommand;

namespace BookingApp.WPF.ViewModel.TourGuide;

public class PickDateForTourRequestViewModel : ViewModelBase

{
    private TourGuideWindow _tourGuideWindow;
    private TourRequest _currentRequest;
    private string _errorMessage;
    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            if (value != _errorMessage)
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }
    }
    private DispatcherTimer _errorTimer;
    private int _hours;

    public int Hours
    {
        get => _hours;
        set {
            if (value != _hours)
            {
                _hours = value;
                OnPropertyChanged();
            }}
    }
    public DateTime Date { get; set; }
    private IRequestedTourService _requestedTourService;
    private ITourService _tourService;
    private ITourExecutionService _tourExecutionService;
    private ITourRequestService _tourRequestService;

    public ICommand AcceptTourRequestCommand { get; private set; }
    private User _loggedUser;

    public PickDateForTourRequestViewModel(TourGuideWindow tourGuideWindow, TourRequest currentRequest)
    {
        _tourGuideWindow = tourGuideWindow;
        _currentRequest = currentRequest;
        _requestedTourService = Injector.Container.Resolve<IRequestedTourService>();
        _tourService = Injector.Container.Resolve<ITourService>();
        _tourExecutionService = Injector.Container.Resolve<ITourExecutionService>();
        _tourRequestService = Injector.Container.Resolve<ITourRequestService>();
        AcceptTourRequestCommand = new DelegateCommand(AcceptTourRequest);
        var app = Application.Current as App;
        _loggedUser = app.LoggedUser;
        _errorTimer = new DispatcherTimer();
        _errorTimer.Interval = TimeSpan.FromSeconds(5);
        _errorTimer.Tick += ClearErrorMessage;
    }
    private bool ValidateFields()
    {
        if ( Hours < 1 || Hours > 23 || Hours == null )
            
        {
            ShowErrorMessage ( "Error: You must enter valid number between 1 and 23 for starting hours.");
            return false;
        }
        if (Date <= DateTime.Today )
            
        {
            ShowErrorMessage ( "Error: Tour scheduling is only available for dates in the future.");
            return false;
        }
        if (Date == null )
            
        {
            ShowErrorMessage ( "Error: You must enter a start date for the tour.");
            return false;
        }
        
        return true;
    }
    
    private void ShowErrorMessage(string message)
    {
        ErrorMessage = message;
        _errorTimer.Start(); 
    }

    private void ClearErrorMessage(object sender, EventArgs e)
    {
        ErrorMessage = "";
        _errorTimer.Stop(); 
    }

    private void AcceptTourRequest(object o)
    {
        if (ValidateFields())
        {
            DateTime proposedStartDate =
                new DateTime(Date.Year, Date.Month, Date.Day, Hours, 0, 0);

            if (!CanAcceptRequest(proposedStartDate))
            {
                MessageBox.Show("This date is not available. Choose different date.");
                return;
            }

            RequestedTour requestedTour = new RequestedTour();
            requestedTour.TourRequestId = _currentRequest.Id;
            requestedTour.Date = Date;
            requestedTour.TourGuideId = _loggedUser.Id;
            _requestedTourService.Save(requestedTour);

            _currentRequest.Status = TourRequestsStatus.Accepted;
            _tourRequestService.Update(_currentRequest);
            
            MessageBox.Show("Tour successfully scheduled!");
        
            if (_tourGuideWindow != null && _tourGuideWindow.MainFrameTourGuideWindow != null)
            {
                _tourGuideWindow.MainFrameTourGuideWindow.Navigate(new HomePage(_tourGuideWindow));
            }
        }
    }

    private bool GuideIsAvailable(DateTime proposedStartDate)
    {
        
        foreach (var tour in _tourService.GetByTourGuideId(_loggedUser.Id))
        {
            foreach (var execution in _tourExecutionService.GetTourExecutions(tour.Id))
            {
               
                if (proposedStartDate  == execution.StartDate)
                {
                    return false;
                }
            }
        }

        foreach (var reqestedTour in _requestedTourService.GetAll())
        {
            if (proposedStartDate == reqestedTour.Date)
            {
                return false;
            }
        }

        return true;
    }

    private bool CanAcceptRequest(DateTime proposedStartDate)
    {
        if (Date >= _currentRequest.BeginDate && Date <= _currentRequest.EndDate &&
            GuideIsAvailable(proposedStartDate))
        {
            return true;
        }

        return false;
    }
}