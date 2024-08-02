using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Dto;
using BookingApp.Utils;

namespace BookingApp.WPF.View.TouristPages;

public partial class TourDetailsPage : Page, INotifyPropertyChanged
{
    private TouristWindow _touristWindow;

    private ITourService _tourService;
    private TourDetailsDto _tourDetailsDto;
    private int _currentTourExecutionId;
    private int _currentPhotoIndex = 0;
    private User _loggedUser;

    
    public string CurrentPhoto => TourDetailsDto?.Photos.ElementAtOrDefault(CurrentPhotoIndex);

    public ICommand NextPhotoCommand { get; }
    public ICommand PreviousPhotoCommand { get; }
    
    public TourDetailsPage(int currentTourExecutionId)
    {
        InitializeComponent();
        this.DataContext = this;
        
        var app = Application.Current as App;
        _tourService = Injector.Container.Resolve<ITourService>();
        _currentTourExecutionId = currentTourExecutionId;
        _loggedUser = app.LoggedUser;
        _tourDetailsDto = _tourService.GetTourDetailsDtos(_currentTourExecutionId);
        NextPhotoCommand = new RelayCommand(_ => NextPhoto(), _ => CanNavigateToNextPhoto());
        PreviousPhotoCommand = new RelayCommand(_ => PreviousPhoto(), _ => CanNavigateToPreviousPhoto());
    }

    private bool CanNavigateToNextPhoto() => CurrentPhotoIndex < TourDetailsDto.Photos.Count - 1;
    private bool CanNavigateToPreviousPhoto() => CurrentPhotoIndex > 0;

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
    
    public TourDetailsDto TourDetailsDto
    {
        get { return _tourDetailsDto; }
        set
        {
            if (value != _tourDetailsDto)
            {
                _tourDetailsDto = value;
                OnPropertyChanged(nameof(TourDetailsDto));
            }
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
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private void ReserveButton_Click(object sender, RoutedEventArgs e)
    {
      
            bool alreadyReserved = _tourService.CheckIfTourAlreadyReserved(_loggedUser.Id, _currentTourExecutionId);

            if (alreadyReserved)
            {
                MessageTextBlock.Text = "You have already reserved this tour.";
                MessagePanel.Visibility = Visibility.Visible;
                
                var timer = new System.Windows.Threading.DispatcherTimer { Interval = TimeSpan.FromSeconds(4) };
                timer.Tick += (sender, args) =>
                {
                    MessagePanel.Visibility = Visibility.Collapsed;
                    timer.Stop();
                };
                timer.Start();

                return;
            }
            //da li tura ima 1 ili 0 slobodnih mesta?
            if (!_tourService.IsFull(_currentTourExecutionId))
            {
                this.NavigationService?.Navigate(new AlternativesPage(_currentTourExecutionId));
                return;
            }
            
            this.NavigationService?.Navigate(new ReservePage(_currentTourExecutionId));
    }
  
    

}