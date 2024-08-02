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
using BookingApp.Parameters;
using BookingApp.Utils;

namespace BookingApp.WPF.View.TouristPages;

public partial class HomePage : Page, INotifyPropertyChanged
{
    private TouristWindow _touristWindow;
  //  private TourController _tourController;
    private ITourService _tourService;
    private List<TourDto> _allToursDtoList;
  //  private TourReservationController _tourReservationController;
    private ITourReservationService _tourReservationService;
    private User _loggedUser;
    private DispatcherTimer messageTimer;

    public HomePage()
    {
        InitializeComponent();
        DataContext = this;

        var app = Application.Current as App;
        //_tourController = app.TourController;
        _tourService = Injector.Container.Resolve<ITourService>();
        _tourReservationService = Injector.Container.Resolve<ITourReservationService>();
        
        AllToursDto = _tourService.GetTourDtos().ToList();
        _loggedUser = app.LoggedUser;
        
        CountryTextBox.Text = "Country";
        CityTextBox.Text = "City";
        DurationTextBox.Text = "Duration";
        LanguageComboBox.SelectedIndex = 0;

        messageTimer = new DispatcherTimer();
        messageTimer.Interval = TimeSpan.FromSeconds(4);
        messageTimer.Tick += MessageTimer_Tick;
        
        FilterTours(); 
    }
    private void MessageTimer_Tick(object sender, EventArgs e)
    {
        MessageVisibility = Visibility.Collapsed;
        OnPropertyChanged(nameof(MessageVisibility));

        messageTimer.Stop();
    }

    public Visibility MessageVisibility
    {
        get => _messageVisibility;
        set
        {
            if (_messageVisibility != value)
            {
                _messageVisibility = value;
                OnPropertyChanged(nameof(MessageVisibility));

                // Startujemo timer kada se postavi vidljivost na Visible
                if (_messageVisibility == Visibility.Visible)
                {
                    messageTimer.Start();
                }
            }
        }
    }
    

    private Visibility _messageVisibility = Visibility.Collapsed;
    public string MessageText { get; set; } = "";

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
                OnPropertyChanged(nameof(AllToursDto));
            }
        }
    }

    private void FilterTours()
    {
        var filterCriteria = new FilterTours
        {
            Country = CountryTextBox.Text.Trim() == "Country" ? "" : CountryTextBox.Text.Trim(),
            City = CityTextBox.Text.Trim() == "City" ? "" : CityTextBox.Text.Trim(),
            // Proveravamo da li DurationTextBox sadrži defaultni tekst "Duration" ili ne može da se parsira u double
            Duration = (DurationTextBox.Text.Trim() == "Duration" || !double.TryParse(DurationTextBox.Text.Trim(), out double duration)) ? 0 : duration,
            PeopleNumber = int.TryParse(PeopleNumberTextBox.Text, out int peopleNumber) ? peopleNumber : 0,
            Language = (LanguageComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString()
        };
        
        if (filterCriteria.Language == "None")
        {
            filterCriteria.Language = null; // ili neka druga logika koja ukazuje na poništavanje filtera
        }

        var filteredTours = _tourService.GetFilteredTours(filterCriteria).ToList();

        AllToursDto = filteredTours;
    }

    
    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        FilterTours();
    }

    private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        FilterTours();
    }

    private void IncreasePeopleNumber_Click(object sender, RoutedEventArgs e)
    {
        UpdatePeopleNumber(true);
    }

    private void DecreasePeopleNumber_Click(object sender, RoutedEventArgs e)
    {
        UpdatePeopleNumber(false);
    }

    private void UpdatePeopleNumber(bool increase)
    {
        if (int.TryParse(PeopleNumberTextBox.Text, out int currentValue))
        {
            currentValue = increase ? currentValue + 1 : Math.Max(currentValue - 1, 1);
            PeopleNumberTextBox.Text = currentValue.ToString();
            FilterTours(); 
        }
    }
    
    private void DetailsButton_Click(object sender, RoutedEventArgs e)
    {
        Button reserveButton = sender as Button;
        TourDto selectedTour = reserveButton.DataContext as TourDto;
        if (selectedTour != null)
        {
            this.NavigationService?.Navigate(new TourDetailsPage(selectedTour.Id));
        }
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
                // Prikazujemo poruku
                MessageTextBlock.Text = "You have already reserved this tour.";
                MessagePanel.Visibility = Visibility.Visible;

                // Postavljamo timer da sakrijemo poruku nakon 4 sekunde
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
    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        TextBox textBox = sender as TextBox;
        string defaultText = textBox.Name switch
        {
            "CountryTextBox" => "Country",
            "CityTextBox" => "City",
            "DurationTextBox" => "Duration",
            _ => string.Empty
        };

        if (textBox != null && textBox.Text == defaultText)
        {
            textBox.Text = ""; // Čisti TextBox ako sadrži defaultni tekst
        }
    }

    private void TextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        TextBox textBox = sender as TextBox;
        if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
        {
            // Ponovo postavlja defaultni tekst ako je TextBox prazan
            switch (textBox.Name)
            {
                case "CountryTextBox":
                    textBox.Text = "Country";
                    break;
                case "CityTextBox":
                    textBox.Text = "City";
                    break;
                case "DurationTextBox":
                    textBox.Text = "Duration";
                    break;
            }
        }
    }


}

