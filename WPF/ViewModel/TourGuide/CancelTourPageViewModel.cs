using System;
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

namespace BookingApp.WPF.ViewModel.TourGuide;

public class CancelTourPageViewModel : ViewModelBase
{
    private TourGuideWindow _tourGuideWindow;

    //injektovanje
    private ITourReservationService _tourReservationService;
    private ITourService _tourService;
    private ITourExecutionService _tourExecutionService;
    private IUserService _userService;
    private IVoucherService _voucherService;

    // ******
    /*private TourReservationController _tourReservationController;
    private TourController _tourController;
    private TourExectuionController _tourExecutionController;
    private UserController _userController;*/
    private User _loggedUser;
    private ObservableCollection<TourDto> _selectedTours;
    /*private VoucherController _voucherController;*/

    public ObservableCollection<TourDto> SelectedTours
    {
        get => _selectedTours;
        set
        {
            if (value != _selectedTours)
            {
                _selectedTours = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand CancelTourCommand { get; private set; }


    private void InitializeTours()
    {
        SelectedTours = new ObservableCollection<TourDto>(
            _tourService.GetCancelableToursByTourGuideId(_loggedUser.Id));
    }


    private void CancelTour(object param)
    {
        int tourId = (int)param;

        if (tourId != null)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel this tour?", "Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                TourExecution currentTour = _tourExecutionService.GetById(tourId);
                foreach (var tourist in currentTour.Tourists)
                {
                    Voucher voucher = new Voucher
                    {
                        TouristId = tourist.TouristId,
                        TourGuideId = _loggedUser.Id,
                        Reason = Reason.TourCancelled,
                        ExpirationDate = DateTime.Now.AddYears(1),
                        IsValid = true
                    };

                    _voucherService.Save(voucher);
                }

                TourDto deletedTour = _selectedTours.FirstOrDefault(tourDto => tourDto.Id == tourId);
                _selectedTours.Remove(deletedTour);
                _tourExecutionService.Delete(currentTour);

                MessageBox.Show("Tour successfully canceled.", "Success", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
    }

    public CancelTourPageViewModel(TourGuideWindow tourGuideWindow)
    {
        //****
        _tourService = Injector.Container.Resolve<ITourService>();
        _tourReservationService = Injector.Container.Resolve<ITourReservationService>();
        _tourExecutionService = Injector.Container.Resolve<ITourExecutionService>();
        _userService = Injector.Container.Resolve<IUserService>();
        _voucherService = Injector.Container.Resolve<IVoucherService>();
        //****
        var app = Application.Current as App;
       // _tourController = app.TourController;
        _tourGuideWindow = tourGuideWindow;
       // _tourExecutionController = app.TourExectuionController;
       // _userController = app.UserController;
        //_tourReservationController = app.TourReservationController;
        _loggedUser = app.LoggedUser;
        //_voucherController = app.VoucherController;
        CancelTourCommand = new DelegateCommand(CancelTour);
        InitializeTours();
    }
}