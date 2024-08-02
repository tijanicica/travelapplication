using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;
using BookingApp.WPF.View.TouristPages;
using Microsoft.Win32;

namespace BookingApp.WPF.ViewModel.Tourist;

public class RateTourViewModel : ViewModelBase
{
    private User _loggedUser;
    private int _currentTourExecutionId;
    private ITourReviewService _tourReviewService;
    private string _tourName;
    private ITourService _tourService;
    private TouristWindow _touristWindow;
    
    public ObservableCollection<string> uploadedFiles { get; } = new ObservableCollection<string>();

    public RateTourViewModel(TouristWindow touristWindow, int currentTourExectuionId)
    {
        _touristWindow = touristWindow;
        
        var app = Application.Current as App;
        _loggedUser = app.LoggedUser;
        _tourReviewService = Injector.Container.Resolve<ITourReviewService>();
        _currentTourExecutionId = currentTourExectuionId;
        _tourService = Injector.Container.Resolve<ITourService>();
        _tourName = _tourService.GetById(_tourService.GetTourIdByExecutionId(currentTourExectuionId)).Name;
        UpdateSubmitButtonAvailability();
        
        CancelCommand = new DelegateCommand(CancelAction);
        SubmitCommand = new DelegateCommand(SubmitAction);
        RemoveFileCommand = new DelegateCommand(RemoveFileAction);
        UploadPhotosCommand = new DelegateCommand(UploadPhotos);
    }
    
    public ICommand CancelCommand { get; private set; }
    public ICommand SubmitCommand { get; private set; }
    public ICommand RemoveFileCommand { get; private set; }
    public ICommand UploadPhotosCommand { get; private set; }
    
    public string TourName
    {
        get { return _tourName; }
        set
        {
            if (_tourName != value)
            {
                _tourName = value;
                OnPropertyChanged(nameof(TourName));
            }
        }
    }
    public string? Photos { get; set;  }
   
    private string _guidesKnowledge;
    public string GuidesKnowledge
    {
        get => _guidesKnowledge;
        set
        {
            if (_guidesKnowledge != value)
            {
                _guidesKnowledge = value;
                ValidateRating(value, nameof(GuidesKnowledge));
                OnPropertyChanged(nameof(GuidesKnowledge));
                UpdateSubmitButtonAvailability(); // Ovo osigurava da se proveri dostupnost Submit dugmeta svaki put kada se promeni vrednost
            }
        }
    }
    
    private string _languageSkills;
    public string LanguageSkills
    {
        get => _languageSkills;
        set
        {
            if (_languageSkills != value)
            {
                _languageSkills = value;
                ValidateRating(value, nameof(LanguageSkills));
                OnPropertyChanged(nameof(LanguageSkills));
                UpdateSubmitButtonAvailability();
            }
        }
    }

    
    private string _amusementLevel;
    public string AmusementLevel
    {
        get => _amusementLevel;
        set
        {
            if (_amusementLevel != value)
            {
                _amusementLevel = value;
                ValidateRating(value, nameof(AmusementLevel));
                OnPropertyChanged(nameof(AmusementLevel));
                UpdateSubmitButtonAvailability();
            }
        }
    }

    private string _comment;
    public string Comment
    {
        get => _comment;
        set
        {
            if (_comment != value)
            {
                _comment = value;
                OnPropertyChanged(nameof(Comment));
                UpdateSubmitButtonAvailability();
            }
        }
    }

    
    

   

    private void UploadPhotos(object obj)
    {
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string[] filePaths = openFileDialog.FileNames;

                foreach (string filePath in filePaths)
                {
                    string[] photos = filePath.Split("\\");
                    uploadedFiles.Add(photos[^1]);
                }

                string photosString = string.Join("*", filePaths);

                Photos = photosString;
                UpdateSubmitButtonAvailability();
            }
        }
    }

    private void SubmitAction(object param)
    {
        TourReview tourReview = new TourReview();
        tourReview.Comment = Comment;
        tourReview.Photos = Photos.Split("*").ToList();
        tourReview.AmusementLevel = int.Parse(AmusementLevel);
        tourReview.GuidesKnowledge = int.Parse(GuidesKnowledge);
        tourReview.GuidesLanguageSkills = int.Parse(LanguageSkills);
        tourReview.IsValid = true;
        tourReview.TouristId = _loggedUser.Id;
        tourReview.TourExecutionId = _currentTourExecutionId;
        tourReview.TourGuideId = _tourService
            .GetById(_tourService.GetTourIdByExecutionId(_currentTourExecutionId)).TourGuideId;

        _tourReviewService.Save(tourReview);
        
       
        _touristWindow.MainFrame.NavigationService.Navigate(new MyToursPage
        {
            DataContext = new MyToursViewModel(_touristWindow) 
            {
                RatingSubmissionSuccess = true 
            }
        });

    }

    private void CancelAction(object param)
    {
     
      _touristWindow.MainFrame.NavigationService.Navigate(new MyToursPage
      {
          DataContext = new MyToursViewModel(_touristWindow)
      });
      
    }
    
    private void RemoveFileAction(object param)
    {
        if (param is string filePath && uploadedFiles.Contains(filePath))
        {
            uploadedFiles.Remove(filePath);
        }
    }

    private void ValidateRating(string ratingText, string propertyName)
    {
        bool isValid = int.TryParse(ratingText, out int rating) && rating >= 1 && rating <= 5;
        string errorMessage = isValid ? "" : "Enter a number between 1 and 5.";

        switch (propertyName)
        {
            case nameof(GuidesKnowledge):
                GuidesKnowledgeError = errorMessage;
                break;
            case nameof(LanguageSkills):
                LanguageSkillsError = errorMessage;
                break;
            case nameof(AmusementLevel):
                AmusementLevelError = errorMessage;
                break;
        }
    }

  
    private void UpdateSubmitButtonAvailability()
    {
        bool isKnowledgeValid = int.TryParse(GuidesKnowledge, out int knowledgeRating) && knowledgeRating >= 1 && knowledgeRating <= 5;
        bool isLanguageSkillsValid = int.TryParse(LanguageSkills, out int languageRating) && languageRating >= 1 && languageRating <= 5;
        bool isAmusementLevelValid = int.TryParse(AmusementLevel, out int amusementRating) && amusementRating >= 1 && amusementRating <= 5;
        bool isCommentValid = !string.IsNullOrWhiteSpace(Comment);
        bool arePhotosUploaded = uploadedFiles.Any(); 

        IsSubmitEnabled = isKnowledgeValid && isLanguageSkillsValid && isAmusementLevelValid && isCommentValid && arePhotosUploaded;
    }
    private bool _isSubmitEnabled;
    public bool IsSubmitEnabled
    {
        get => _isSubmitEnabled;
        set
        {
            _isSubmitEnabled = value;
            OnPropertyChanged(nameof(IsSubmitEnabled)); 
        }
    }


    private Visibility _overlayGridSubmitVisibility = Visibility.Collapsed; 
    public Visibility OverlayGridSubmitVisibility
    {
        get => _overlayGridSubmitVisibility;
        set
        {
            _overlayGridSubmitVisibility = value;
            OnPropertyChanged(nameof(OverlayGridSubmitVisibility)); 
        }
    }
    
    private string _guidesKnowledgeError;
    public string GuidesKnowledgeError
    {
        get => _guidesKnowledgeError;
        set
        {
            _guidesKnowledgeError = value;
            OnPropertyChanged(nameof(GuidesKnowledgeError));
        }
    }

    private string _languageSkillsError;
    public string LanguageSkillsError
    {
        get => _languageSkillsError;
        set
        {
            _languageSkillsError = value;
            OnPropertyChanged(nameof(LanguageSkillsError));
        }
    }

    private string _amusementLevelError;
    public string AmusementLevelError
    {
        get => _amusementLevelError;
        set
        {
            _amusementLevelError = value;
            OnPropertyChanged(nameof(AmusementLevelError));
        }
    }




}