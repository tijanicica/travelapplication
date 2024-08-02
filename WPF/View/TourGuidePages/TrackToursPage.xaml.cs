using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Dto;
using BookingApp.Utils;

namespace BookingApp.WPF.View.TourGuidePages;

public partial class TrackToursPage : Page, INotifyPropertyChanged
{
    private TourGuideWindow _tourGuideWindow = null;
    /*private TourController _tourController;

    private TourExectuionController _tourExectuionController;*/
    private ITourService _tourService;
    private ITourExecutionService _tourExecutionService;
    private User _loggedUser;
    private ObservableCollection<TourDto> _selectedTours;
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

   
   public TrackToursPage(TourGuideWindow tourGuideWindow)
   {
       InitializeComponent();
       this.DataContext = this;
       _tourGuideWindow = tourGuideWindow;
       var app = Application.Current as App;
       _loggedUser = app.LoggedUser;
       /*_tourController = app.TourController;
       _tourExectuionController = app.TourExectuionController;*/
       _tourService = Injector.Container.Resolve<ITourService>();
       _tourExecutionService = Injector.Container.Resolve<ITourExecutionService>();
       InitializeTours();

   }


   private void InitializeTours()
   {
       SelectedTours = new ObservableCollection<TourDto>(
           _tourService.GetToursTodayByTourGuideId( _loggedUser.Id)
               .Where(tour => tour.StartDate >= DateTime.Now && _tourExecutionService.GetById(tour.Id).Tourists.Any() &&
                              (_tourExecutionService.GetById(tour.Id).Status == Status.Inactive || _tourExecutionService.GetById(tour.Id).Status == Status.Started )));
   }
    
    
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
        private void OpenTrackOneTourPage(object sender, RoutedEventArgs e)
        {
            Button startTourButton = sender as Button;

            TourDto currentTour = startTourButton.DataContext as TourDto;

            if (currentTour != null)
            {
                foreach (var tourExecution in _tourExecutionService.GetAll())
                {
                    if (tourExecution.Status == Status.Started && tourExecution.Id != currentTour.Id)
                    {
                        MessageBox.Show("You can't have 2 tours active at the same time"); 
                        return;
                    }
                }
                TourExecution execution = _tourExecutionService.GetById(currentTour.Id);
                execution.Status = Status.Started;
                _tourExecutionService.Update(execution);
                this.NavigationService?.Navigate(new TrackOneTourPage(_tourGuideWindow, currentTour));

            }
        }
    
}