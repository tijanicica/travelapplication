using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Dto;
using BookingApp.Utils;

namespace BookingApp.WPF.View.TouristPages;

public partial class AlternativesPage : Page, INotifyPropertyChanged
{
    private TouristWindow _touristWindow;
    private ITourService _tourService;
    private List<TourDto> _allToursDtoList;
    private int _currentTourExecutionId;
    private User _loggedUser;
    public AlternativesPage(int currentTourExecutionId)
    {
        InitializeComponent();
        this.DataContext = this;
        
        var app = Application.Current as App;
        _tourService = Injector.Container.Resolve<ITourService>();
        _currentTourExecutionId = currentTourExecutionId;
        _allToursDtoList = _tourService.GetTourDtosByLocation(_currentTourExecutionId).ToList();
        _loggedUser = app.LoggedUser;
       
        if (_allToursDtoList.Count == 0)
        {
            
            ToursDataGrid.Visibility = Visibility.Collapsed;
            AlternativesHeader.Visibility = Visibility.Collapsed; 
            NoAlternativesMessage.Visibility = Visibility.Visible;
        }
        else
        {
            ToursDataGrid.Visibility = Visibility.Visible;
            AlternativesHeader.Visibility = Visibility.Visible;
            NoAlternativesMessage.Visibility = Visibility.Collapsed; 
        }
        
        DispatcherTimer timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(4);
        timer.Tick += Timer_Tick;
        timer.Start();
        
        MessageBorder.Visibility = Visibility.Visible;
    }
    private void Timer_Tick(object sender, EventArgs e)
    {
        MessageBorder.Visibility = Visibility.Collapsed;
        (sender as DispatcherTimer).Stop();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public List<TourDto> AllToursDto
    {
        get { return _allToursDtoList; }
        set
        {
            if (value != _allToursDtoList)
            {
                _allToursDtoList = value;
                OnPropertyChanged("AllToursDto");
            }
        }
    }
    private void DetailsButton_Click(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void ReserveButton_Click(object sender, RoutedEventArgs e)
    {
        Button reserveButton = sender as Button;
        TourDto selectedTour = reserveButton.DataContext as TourDto;

        if (selectedTour != null)
        {
            bool alreadyReserved = _tourService.CheckIfTourAlreadyReserved(_loggedUser.Id, selectedTour.Id);

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
            
            if (!_tourService.IsFull(selectedTour.Id))
            {
                this.NavigationService?.Navigate(new AlternativesPage(selectedTour.Id));
                return;
            }
            
            this.NavigationService?.Navigate(new ReservePage(selectedTour.Id));
        }
    }
}