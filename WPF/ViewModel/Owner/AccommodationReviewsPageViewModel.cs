using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;
using BookingApp.WPF.View.OwnerPages;
using DelegateCommand = BookingApp.WPF.View.DelegateCommand;

namespace BookingApp.WPF.ViewModel.Owner;

public class AccommodationReviewsPageViewModel: ViewModelBase
{
    private OwnerWindow _ownerWindow;
    private readonly IAccommodationReviewService _accommodationReviewService;
    //private readonly IAccommodationReservationService _accommodationReservationService;
  
    private List<AccomodationReviewModelViewModel> _accomodationReviews;
    private IAccommodationService _AccomodationService;
    private User _loggedUser;
    private IUserService _userService;
    public ICommand ViewCommand { get; private set; }


    public AccommodationReviewsPageViewModel(OwnerWindow ownerWindow)
    {
        _ownerWindow = ownerWindow;
        var app = Application.Current as App;
        _accommodationReviewService = Injector.Container.Resolve<IAccommodationReviewService>();
        _userService = Injector.Container.Resolve<IUserService>();
        //_accommodationReservationService = Injector.Container.Resolve<IAccommodationReservationService>();
        _loggedUser = app.LoggedUser;
      
        ViewCommand = new DelegateCommand(View);
        InitializeAccomodationReviews();
        
    }
    private void View(object param)
    {
        // Button viewMoreButton = sender as Button;
        //TourReviewsViewModel selectedReview = viewMoreButton?.DataContext as TourReviewsViewModel;
        int ratingId = (int)param;
        if (ratingId != null)
        {
           AccommodationReview currentReview = _accommodationReviewService.GetById(ratingId);
            _ownerWindow.MainFrameOwnerWindow.NavigationService.Navigate(new OneAccommodationReviewPagee
            {
                DataContext = new OneAccommodationReviewPageViewModel(_ownerWindow,
                    currentReview)
            });
            
        }

    }
    public List<AccomodationReviewModelViewModel> AccommodationReviews
    {
        get => _accomodationReviews;
        set
        {
            if (value != _accomodationReviews)
            {
                _accomodationReviews = value;
                OnPropertyChanged();
            }
        }
    }
    private void InitializeAccomodationReviews()
    {
       
        AccommodationReviews =  new List<AccomodationReviewModelViewModel>();
        foreach (var review in _accommodationReviewService.GetByOwnerIdFiltered(_loggedUser.Id))
        {
            AccomodationReviewModelViewModel accomodationReviewModelViewModel = new AccomodationReviewModelViewModel();
            accomodationReviewModelViewModel.accommodationCleanliness = review.AccommodationCleanliness;
            accomodationReviewModelViewModel.guestsName = _userService.GetUsernameById(review.GuestID);
            accomodationReviewModelViewModel.ownerCorrectness = review.OwnerCorrectness;
            accomodationReviewModelViewModel.additionalComment = review.AdditionalComment;
            accomodationReviewModelViewModel.ownerID = _loggedUser.Id;
            accomodationReviewModelViewModel.ratingID = review.RatingID;
            AccommodationReviews.Add(accomodationReviewModelViewModel);
        }
        
    }


}