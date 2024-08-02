using System.Collections.ObjectModel;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Service;
using BookingApp.Utils;
using BookingApp.WPF.View;
using BookingApp.WPF.View.TourGuidePages;
using BookingApp.WPF.ViewModel.Tourist;
using DelegateCommand = BookingApp.WPF.View.OwnerPages.DelegateCommand;

namespace BookingApp.WPF.ViewModel.TourGuide;

public class AllComplexRequestsPageViewModel : ViewModelBase
{
    private readonly TourGuideWindow _tourGuideWindow;
    private readonly IComplexTourRequestService _complexTourRequestService;
    private readonly IUserService _userService;
    

    private ObservableCollection<ComplexTourRequestsModelViewModel> _complexTourRequests;

    public ObservableCollection<ComplexTourRequestsModelViewModel> ComplexTourRequests
    {
        get => _complexTourRequests;
        set
        {
            if (value != _complexTourRequests)
            {
                _complexTourRequests = value;
                OnPropertyChanged();
            }
        }
    }
    public AllComplexRequestsPageViewModel(TourGuideWindow tourGuideWindow)
    {
        _tourGuideWindow = tourGuideWindow;
        _complexTourRequestService =  Injector.Container.Resolve<IComplexTourRequestService>();
        _userService =  Injector.Container.Resolve<IUserService>();
        InitializeRequests();
        OpenComplexTourPartsPageCommand = new DelegateCommand(OpenComplexTourPartsPage);
    }

    private void InitializeRequests()
    {
        ComplexTourRequests = new ObservableCollection<ComplexTourRequestsModelViewModel>();
        foreach (var request in _complexTourRequestService.GetAll())
        {
            ComplexTourRequests.Add(new ComplexTourRequestsModelViewModel("Request by: " + _userService.GetUsernameById(request.TouristId), request.Id));
        }
    }
    
    public ICommand OpenComplexTourPartsPageCommand { get; private set; }

    private void OpenComplexTourPartsPage(object param)
    {
        int requestId = (int)param;
        if (requestId != null)
        {
            ComplexTourRequest currentRequest = _complexTourRequestService.GetById(requestId);
            _tourGuideWindow.MainFrameTourGuideWindow.NavigationService.Navigate(new ComplexTourPartsPage
            {
                DataContext = new ComplexTourPartsPageViewModel(_tourGuideWindow,
                    currentRequest)
            });
        }


    }
    
    /* private void OpenOneTourRequestPage(object param)
    {
        
        // Button viewMoreButton = sender as Button;
        //TourReviewsViewModel selectedReview = viewMoreButton?.DataContext as TourReviewsViewModel;
        int requestId = (int)param;
        if (requestId != null)
        {
            TourRequest currentRequest = _tourRequestService.GetById(requestId);
            _tourGuideWindow.MainFrameTourGuideWindow.NavigationService.Navigate(new OneTourRequestPage
            {
                DataContext = new OneTourRequestPageViewModel(_tourGuideWindow,
                    currentRequest)
            });
            
        }
    }*/
    
    
    
    
    
}