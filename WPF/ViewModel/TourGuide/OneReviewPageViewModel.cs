using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;

namespace BookingApp.WPF.ViewModel.TourGuide;

public class OneReviewPageViewModel : ViewModelBase
{
    private TourGuideWindow _tourGuideWindow;
    private TourReview _currentReview;

    private string _touristName;

    public string TouristName
    {
        get => _touristName;
        set {
            if (value != _touristName)
            {
                _touristName = value;
                OnPropertyChanged();
            }}
    }

    private string _tourName;
    public string TourName  {
        get => _tourName;
        set {
            if (value != _tourName)
            {
                _tourName = value;
                OnPropertyChanged();
            }}
    }

    private int _guidesKnowledge;
    public int GuidesKnowledge{
        get => _guidesKnowledge;
        set {
            if (value != _guidesKnowledge)
            {
                _guidesKnowledge = value;
                OnPropertyChanged();
            }}
    }

    private int _guidesLanguage;
    public int GuidesLanguage {
        get => _guidesLanguage;
        set {
            if (value != _guidesLanguage)
            {
                _guidesLanguage = value;
                OnPropertyChanged();
            }}
    }

    private int _amusementLevel;
    public int AmusementLevel {
        get => _amusementLevel;
        set {
            if (value != _amusementLevel)
            {
                _amusementLevel = value;
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
    
    
    public List<string> Photos;

    /*private TourController _tourController;
    private TourExectuionController _tourExectuionController;
    private UserController _userController;*/

    private ITourService _tourService;
    private ITourExecutionService _tourExecutionService;
    private IUserService _userService;
    
    public OneReviewPageViewModel(TourGuideWindow tourGuideWindow, TourReview currentReview)
    {
        
        var app = Application.Current as App;
        _tourGuideWindow = tourGuideWindow;
        _currentReview = currentReview;
        /*_userController = app.UserController;
        _tourExectuionController = app.TourExectuionController;
        _tourController = app.TourController;*/
        _tourService = Injector.Container.Resolve<ITourService>();
        _tourExecutionService = Injector.Container.Resolve<ITourExecutionService>();
        _userService = Injector.Container.Resolve<IUserService>();
        InitializeReview();

        NextPhotoCommand = new RelayCommand(_ => NextPhoto(), _ => CanNavigateToNextPhoto());
        PreviousPhotoCommand = new RelayCommand(_ => PreviousPhoto(), _ => CanNavigateToPreviousPhoto());
        
        //InitializeReview();

    }
    
    

    private void InitializeReview()
    {
        TouristName = _userService.GetUsernameById(_currentReview.TouristId);
        TourName = _tourService.GetById(_tourService.GetTourIdByExecutionId(_currentReview.TourExecutionId)).Name;
        GuidesKnowledge = _currentReview.GuidesKnowledge;
        GuidesLanguage = _currentReview.GuidesLanguageSkills;
        AmusementLevel = _currentReview.AmusementLevel;
        Comment = _currentReview.Comment;
        Photos = _currentReview.Photos;
    }
}