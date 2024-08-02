using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class RateAccommodationViewModel : ViewModelBase
    {
        private int _cleanlinessRating;
        private int _ownerCorrectnessRating;
        private string _additionalComment;
        private ObservableCollection<string> _photos;
        private readonly GuestWindow _guestWindow;


        private AccommodationReservation _selectedReservation { get; set; }
        private IAccommodationReviewService _accommodationReviewService;
        private IAccommodationService _accommodationService;
        private IGuestService _guestService;

        public AccommodationReservation SelectedReservation
        {
            get { return _selectedReservation; }
            set
            {
                _selectedReservation = value;
                OnPropertyChanged(nameof(SelectedReservation));
            }
        }

        public int CleanlinessRating
        {
            get => _cleanlinessRating;
            set
            {
                _cleanlinessRating = value;
                OnPropertyChanged(nameof(CleanlinessRating));
            }
        }

        public int OwnerCorrectnessRating
        {
            get => _ownerCorrectnessRating;
            set
            {
                _ownerCorrectnessRating = value;
                OnPropertyChanged(nameof(OwnerCorrectnessRating));
            }
        }

        public string AdditionalComment
        {
            get => _additionalComment;
            set
            {
                _additionalComment = value;
                OnPropertyChanged(nameof(AdditionalComment));
            }
        }

        public ObservableCollection<string> Photos
        {
            get => _photos;
            set
            {
                _photos = value;
                OnPropertyChanged(nameof(Photos));
            }
        }

        public ICommand SubmitRatingCommand { get; }

        public RateAccommodationViewModel(AccommodationReservation selectedReservation, GuestWindow guestWindow)
        {
            SelectedReservation = selectedReservation;
            Photos = new ObservableCollection<string>();
            SubmitRatingCommand = new DelegateCommand(SubmitRating);
            AddPhotoCommand = new DelegateCommand(AddPhoto);
            _accommodationService = Injector.Container.Resolve<IAccommodationService>();
            _accommodationReviewService = Injector.Container.Resolve<IAccommodationReviewService>();
            _guestService = Injector.Container.Resolve<IGuestService>();
            _guestWindow = guestWindow;
            IsRated = false;
        }
        
        private string _newPhoto;
        public string NewPhoto
        {
            get { return _newPhoto; }
            set
            {
                _newPhoto = value;
                OnPropertyChanged(nameof(NewPhoto));
            }
        }

        public ICommand AddPhotoCommand { get; private set; }

        private void AddPhoto(object parameter)
        {
            if (!string.IsNullOrEmpty(NewPhoto))
            {
                Photos.Add(NewPhoto);

                NewPhoto = string.Empty;
            }
        }
        
        private string _renovationRecommendation;
        public string RenovationRecommendation
        {
            get => _renovationRecommendation;
            set
            {
                _renovationRecommendation = value;
                OnPropertyChanged(nameof(RenovationRecommendation));
            }
        }

        private int _renovationUrgencyLevel;
        public int RenovationUrgencyLevel
        {
            get => _renovationUrgencyLevel;
            set
            {
                _renovationUrgencyLevel = value;
                OnPropertyChanged(nameof(RenovationUrgencyLevel));
            }
        }


        private void SubmitRating(object parameter)
        {
            var app = Application.Current as App;
            int currentGuestId = app.LoggedUser.Id;
            var accommodation = _accommodationService.GetAccommodationById(_selectedReservation.AccommodationId);

            var review = new AccommodationReview()
            {
                GuestID = currentGuestId,
                OwnerID = accommodation.OwnerId,
                GuestName = _guestService.GetById(currentGuestId).Username,
                AccommodationCleanliness = CleanlinessRating,
                OwnerCorrectness = OwnerCorrectnessRating,
                AdditionalComment = AdditionalComment,
                Photos = new List<string>(Photos),
                RenovationRecommendation = RenovationRecommendation,
                RenovationUrgencyLevel = RenovationUrgencyLevel,
                AccommodationID = accommodation.Id,
                ReviewDate = DateTime.Now
                
            };

            _accommodationReviewService.Save(review);
            IsRated = true;
            ((DelegateCommand)SubmitRatingCommand).RaiseCanExecuteChanged();
            
            IsRated = true;
            ReservationRated?.Invoke(this, SelectedReservation.Id);

            MessageBox.Show("Your rating has been submitted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            
            _guestWindow.MainFrameGuestWindow.NavigationService?.GoBack();
        }
        
        public event EventHandler<int> ReservationRated;

        private ICommand _removePhotoCommand;
        public ICommand RemovePhotoCommand
        {
            get
            {
                if (_removePhotoCommand == null)
                {
                    _removePhotoCommand = new DelegateCommand(RemovePhoto);
                }
                return _removePhotoCommand;
            }
        }

        private void RemovePhoto(object parameter)
        {
            if (parameter is string photo)
            {
                Photos.Remove(photo);
            }
        }
        
        private bool _isRated;
        public bool IsRated
        {
            get => _isRated;
            set
            {
                _isRated = value;
                OnPropertyChanged(nameof(IsRated));
            }
        }


    }
}
