using System.Collections.Generic;
using System.Linq;
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

public class OneAccommodationReviewPageViewModel : ViewModelBase
{
    private OwnerWindow _ownerWindow;
    private AccommodationReview _currentReview;
  
    private string _guestName;

    public string GuestName
    {
        get => _guestName;
        set {
            if (value != _guestName)
            {
                _guestName = value;
                OnPropertyChanged();
            }}
    }

   
    private int _accommodationCleanliness;
    
    public int AccommodationCleanliness{
        get => _accommodationCleanliness;
        set {
            if (value != _accommodationCleanliness)
            {
                _accommodationCleanliness = value;
                OnPropertyChanged();
            }}
    }

    private int _ownerCorrectness;
    public int OwnerCorrectness {
        get => _ownerCorrectness;
        set {
            if (value != _ownerCorrectness)
            {
                _ownerCorrectness = value;
                OnPropertyChanged();
            }}
    }
    
    private string _comment;
    public string Comment {
        get => _comment;
        set {
            if (value != _comment)
            {
                _comment = value;
                OnPropertyChanged();
            }}
    }
   

    /*private TourController _tourController;
    private TourExectuionController _tourExectuionController;
    private UserController _userController;*/

    private IAccommodationService _accommodationService;
    private IUserService _userService;
    
    public OneAccommodationReviewPageViewModel(OwnerWindow ownerWindow, AccommodationReview currentReview)
    {
        
        var app = Application.Current as App;
        _ownerWindow = ownerWindow;
        _currentReview = currentReview;
        /*_userController = app.UserController;
        _tourExectuionController = app.TourExectuionController;
        _tourController = app.TourController;*/
        _accommodationService = Injector.Container.Resolve<IAccommodationService>();
        _userService = Injector.Container.Resolve<IUserService>();
        
        InitializeReview();
        
        NextPhotoCommand = new RelayCommand(_ => NextPhoto(), _ => CanNavigateToNextPhoto());
        PreviousPhotoCommand = new RelayCommand(_ => PreviousPhoto(), _ => CanNavigateToPreviousPhoto());
        CancelCommand = new DelegateCommand(CancelAction);
       

    }

    private void InitializeReview()
    {
        GuestName = _userService.GetUsernameById(_currentReview.GuestID);
        AccommodationCleanliness = _currentReview.AccommodationCleanliness;
        OwnerCorrectness = _currentReview.OwnerCorrectness;
      
        Comment = _currentReview.AdditionalComment;
        Photos = _currentReview.Photos;
    }
    private int _currentPhotoIndex = 0;

    public string CurrentPhoto => Photos.ElementAtOrDefault(CurrentPhotoIndex);
    
    private void NextPhoto()
    {
        if (CanNavigateToNextPhoto())
        {
            CurrentPhotoIndex++;
        }
    }

    private void PreviousPhoto()
    {
        if (CanNavigateToPreviousPhoto())
        {
            CurrentPhotoIndex--;
        }
    }

    public int CurrentPhotoIndex
    {
        get => _currentPhotoIndex;
        set
        {
            if (_currentPhotoIndex != value)
            {
                _currentPhotoIndex = value;
                OnPropertyChanged(nameof(CurrentPhotoIndex));
                OnPropertyChanged(nameof(CurrentPhoto));
            }
        }
    }
    private bool CanNavigateToNextPhoto() => CurrentPhotoIndex < Photos.Count - 1;
    private bool CanNavigateToPreviousPhoto() => CurrentPhotoIndex > 0;


    public ICommand NextPhotoCommand { get; }
    public ICommand PreviousPhotoCommand { get; }
    public ICommand CancelCommand { get; private set; }

    private void CancelAction(object param)
    {
     
        _ownerWindow.MainFrameOwnerWindow.NavigationService.Navigate(new AccommodationReviews()
        {
            DataContext = new AccommodationReviewsPageViewModel(_ownerWindow)
        });
      
    }
    
    
    public List<string> Photos;
}