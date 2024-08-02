using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;

namespace BookingApp.WPF.View.TourGuidePages;

public partial class AddTourSpotsPage : Page, INotifyPropertyChanged
{
    private Tour _currentTour;
    private TourGuideWindow _tourGuideWindow;
    //private TourController _tourController;
    private ITourService _tourService;
    private User _loggedUser;
    public string? StartSpot { get; set; }
    public string? EndSpot { get; set; }
    
    private string _errorMessage;
    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            if (value != _errorMessage)
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }
    }
    private DispatcherTimer _errorTimer;

    
    public  ObservableCollection<TourSpot> AddedTourSpots { get; set; }
    public string? Description
    {
        get => _description;

        set
        {
            if (_description != value)
            {
                _description = value;
                OnPropertyChanged();
            }
        }
    }

    private string _description;
    
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    } 
    public AddTourSpotsPage(TourGuideWindow tourGuideWindow,Tour currentTour)
    {
        InitializeComponent();
        _tourGuideWindow = tourGuideWindow;
        _currentTour = currentTour;
        this.DataContext = this;
        AddedTourSpots = new ObservableCollection<TourSpot>();
        var app = Application.Current as App;
       // _tourController = app.TourController;
        _tourService = Injector.Container.Resolve<ITourService>();
        _loggedUser = app.LoggedUser;
        _errorTimer = new DispatcherTimer();
        _errorTimer.Interval = TimeSpan.FromSeconds(5);
        _errorTimer.Tick += ClearErrorMessage;



    }
    private bool ValidateFields()
    {
        if (String.IsNullOrEmpty(StartSpot) ||
            String.IsNullOrEmpty(EndSpot))
            
        {
            ShowErrorMessage ( "Error: You must enter both start and end stop.");
            return false;
        }
        
        return true;
    }
    
    private void ShowErrorMessage(string message)
    {
        ErrorMessage = message;
        _errorTimer.Start(); 
    }

    private void ClearErrorMessage(object sender, EventArgs e)
    {
        ErrorMessage = "";
        _errorTimer.Stop(); 
    }

    private void DeleteButtonClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is TourSpot tourSpot)
        {
            AddedTourSpots.Remove(tourSpot);
        }
    }

    private void AddNewTourSpot(object sender, RoutedEventArgs e)
    {
        
            
           if (Description != null) 
                AddedTourSpots.Add(new TourSpot { Description = Description });
           TourSpotTextBox.Clear();
            
        
    }

    private void CreateNewTour(object sender, RoutedEventArgs e)
    {
        if (ValidateFields())
        {
            int i = 0;
            _currentTour.TourSpots = new List<TourSpot>();
            _currentTour.TourSpots.Add(new TourSpot(StartSpot) { Start = true, Description = StartSpot});
            _currentTour.TourSpots.AddRange(AddedTourSpots);
            _currentTour.TourSpots.Add(new TourSpot(EndSpot){End = true, Description = EndSpot});

            _currentTour.TourSpots.ForEach(spot => spot.Id = i++);
            _currentTour.TourGuideId = _loggedUser.Id;
            _tourService.Save(_currentTour);
            MessageBox.Show("Tour successfully added!");
        
            if (_tourGuideWindow != null && _tourGuideWindow.MainFrameTourGuideWindow != null)
            {
                _tourGuideWindow.MainFrameTourGuideWindow.Navigate(new HomePage(_tourGuideWindow));
            }
        }
       
    }

}