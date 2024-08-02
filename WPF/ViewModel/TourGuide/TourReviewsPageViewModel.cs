using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;
using BookingApp.WPF.View.TourGuidePages;

namespace BookingApp.WPF.ViewModel.TourGuide;

public class TourReviewsPageViewModel : ViewModelBase
{
    
     private TourGuideWindow _tourGuideWindow;
    private User _loggedUser;
    private ObservableCollection<TourReviewsViewModel> _tourReviews;

    private ITourReviewService _tourReviewService;
    private ITourExecutionService _tourExecutionService;
    private ITourService _tourService;
    private IUserService _userService;
    
   
    
   
    public ObservableCollection<TourReviewsViewModel> TourReviews
    {
        get => _tourReviews;
        set
        {
            if (value != _tourReviews)
            {
                _tourReviews = value;
                OnPropertyChanged();
            }
        }
    }
   

    private void InitializeTourReviews()
    {
        TourReviews = new ObservableCollection<TourReviewsViewModel>();
        foreach (var review in _tourReviewService.GetByTourGuideId(_loggedUser.Id))
        {
            
            TourExecution execution = _tourExecutionService.GetById(review.TourExecutionId);
            string tourName = _tourService.GetById(_tourService.GetTourIdByExecutionId(review.TourExecutionId)).Name;
            string touristName = _userService.GetUsernameById(review.TouristId);
            
            List<TourSpot> tourSpots = _tourService.GetTourSpotsByTourId(_tourService.GetTourIdByExecutionId(execution.Id))
                    .ToList();
            string joinedAt =  tourSpots.Find(spot => spot.Id == _tourExecutionService.GetTourTouristByTouristIdAndExecutionId(review.TouristId, review.TourExecutionId).JoinedAtTourSpot).Description;
            TourReviews.Add(new TourReviewsViewModel(review.Id,touristName, tourName, joinedAt, review.IsValid));
            
        }
        
        
    }
    
    public ICommand ReportReviewCommand { get; private set; }


    private void ReportReview(object param)
    {
      
        
        int reviewId = (int)param;
        
        if (reviewId != null)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to report this review?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                
                TourReview currentReview = _tourReviewService.GetById(reviewId);
                currentReview.IsValid = false;
                _tourReviewService.Update(currentReview);
                _tourGuideWindow.MainFrameTourGuideWindow.Content = new TourReviewsPage()
                {
                    DataContext = new TourReviewsPageViewModel(_tourGuideWindow)
                };
                
                
                
            }
        }
    }
    public ICommand OpenOneReviewPageCommand { get; private set; }


    private void OpenOneReviewPage(object param)
    {
      
         int reviewId = (int)param;
        if (reviewId != null)
        {
            TourReview currentReview = _tourReviewService.GetById(reviewId);
            _tourGuideWindow.MainFrameTourGuideWindow.NavigationService.Navigate(new OneReviewPage
            {
                DataContext = new OneReviewPageViewModel(_tourGuideWindow,
                    currentReview)
            });
            
        }

    }
    public TourReviewsPageViewModel(TourGuideWindow tourGuideWindow)
    {
       
        _tourGuideWindow = tourGuideWindow;
        var app = Application.Current as App;
        _loggedUser = app.LoggedUser;
        _tourService = Injector.Container.Resolve<ITourService>();
        _tourExecutionService = Injector.Container.Resolve<ITourExecutionService>();
        _tourReviewService = Injector.Container.Resolve<ITourReviewService>();
        _userService = Injector.Container.Resolve<IUserService>();
        
        ReportReviewCommand = new DelegateCommand(ReportReview);
        OpenOneReviewPageCommand = new DelegateCommand(OpenOneReviewPage);

        InitializeTourReviews();
    }
}