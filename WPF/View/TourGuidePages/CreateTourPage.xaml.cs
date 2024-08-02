using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using BookingApp.Domain.Model;
using Microsoft.Win32;

namespace BookingApp.WPF.View.TourGuidePages;

public partial class CreateTourPage : Page, INotifyPropertyChanged
{
    private TourGuideWindow _tourGuideWindow = null;
    private Tour _currentTour;
    private ObservableCollection<string> uploadedFiles = new ObservableCollection<string>();
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

    public CreateTourPage(TourGuideWindow tourGuideWindow, string? city = null, string? language = null)
    {
        InitializeComponent();
        this.DataContext = this;

        _tourGuideWindow = tourGuideWindow;
        UploadedFilesListBox.ItemsSource = uploadedFiles;
        _errorTimer = new DispatcherTimer();
        _errorTimer.Interval = TimeSpan.FromSeconds(5);
        _errorTimer.Tick += ClearErrorMessage;

        if (city != null)
        {
            City = city;
        }

        if (language != null)
        {
            for (int i = 0; i < Languages.Length; i++)
            {
                if (Languages[i] == language)
                {
                    GuidesLanguage = i;
                }
            }
        }
    }

    public CreateTourPage()
    {
    }
    
    private bool ValidateFields()
    {
        if (String.IsNullOrEmpty(TourName) ||
            String.IsNullOrEmpty(City) ||
            String.IsNullOrEmpty(Country) ||
            String.IsNullOrEmpty(Description) ||
            GuidesLanguage == -1 ||
            String.IsNullOrEmpty(Capacity) ||
            String.IsNullOrEmpty(Duration) ||
            Duration == "0" ||
            Capacity == "0")
        {
            ShowErrorMessage ( "Error: All fields marked with * are mandatory and must be filled out correctly.");
            return false;
        }

        if (!int.TryParse(Capacity, out int parsedCapacity) || parsedCapacity <= 0 || parsedCapacity > 250)
        {
            ShowErrorMessage  ("Error: Capacity must be a valid positive number less than 250.");
            return false;
        }

        if (!double.TryParse(Duration, out double parsedDuration) || parsedDuration <= 0 || parsedDuration > 16)
        {
            ShowErrorMessage  ("Error: Duration must be a valid positive number less than 16.");
            return false;
        }

        if (uploadedFiles.Count == 0)
        {
            ShowErrorMessage  ("Error: At least one photo must be uploaded.");
            return false;
        }

        return true;
    }
    private void ShowErrorMessage(string message)
    {
        ErrorMessage = message;
        _errorTimer.Start(); // Start the timer to clear the error message after 5 seconds
    }

    private void ClearErrorMessage(object sender, EventArgs e)
    {
        ErrorMessage = "";
        _errorTimer.Stop(); // Stop the timer
    }
    
    private void OpenAddTourSpotsPage(object sender, RoutedEventArgs e)
    {
        ErrorMessage = "";

        
        if (ValidateFields())
        {
            Enum.TryParse<Language>(Languages[GuidesLanguage], out var type);

            _currentTour = new Tour(TourName, new Location(City, Country), Description, type, Int32.Parse(Capacity),
                Double.Parse(Duration));
            _currentTour.Photos = Photos.Split("*").ToList();
            _tourGuideWindow.MainFrameTourGuideWindow.Content = new AddTourSpotsPage(_tourGuideWindow, _currentTour);
        }
    }

    
    private string _tourName;
    public string? TourName  {
        get => _tourName;
        set {
            if (value != _tourName)
            {
                _tourName = value;
                OnPropertyChanged();
            }}
    }


    public string? Description { get; set; }
    public int GuidesLanguage { get; set; } = -1;
    public string? Capacity { get; set; }
    public string? Duration { get; set; }
    public string? Photos { get; set; }

    public string? City { get; set; }
    public string? Country { get; set; }

    public string[]? Languages { get; set; } = new[] { "English", "Serbian" };
    

    private void UploadPhotos(object sender, MouseButtonEventArgs e)
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
            }
        }
    }

    private void RemoveFile_Click(object sender, RoutedEventArgs e)
    {
        Button button = sender as Button;
        if (button != null)
        {
            string filePath = button.Tag as string;
            if (!string.IsNullOrEmpty(filePath))
            {
                uploadedFiles.Remove(filePath);
            }
        }
    }

    private void OpenCreateTourPage(object sender, RoutedEventArgs e)
    {
        _tourGuideWindow.MainFrameTourGuideWindow.Content = new CreateTourPage(_tourGuideWindow);
    }
    
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    

    
}