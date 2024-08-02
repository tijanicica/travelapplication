using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using Microsoft.Win32;


namespace BookingApp.WPF.View.OwnerPages;

public partial class CreateAccommodationPage : Page
{
    private OwnerWindow _ownerWindow ;
    private Accommodation _currentAccommodation;
    private User _loggedUser;
   // private AccommodationController _accommodationController; 
    private  readonly IAccommodationService _accommodationService;
    public CreateAccommodationPage(OwnerWindow ownerWindow, string? city = null, string? type = null)
    {
        InitializeComponent();
        this.DataContext = this;
        var app = Application.Current as App;
        _loggedUser = app.LoggedUser;
        _accommodationService =  Injector.Container.Resolve<IAccommodationService>();
        UploadedFilesListBox.ItemsSource = uploadedFiles;
        _ownerWindow = ownerWindow;
        
       if (type != null)
       {
           for (int i = 0; i < AccommodationType.Length; i++)
           {
               if (AccommodationType[i] == type)
               {
                   CurrentType = i;
               }
           }
       }
       
       if (city != null)
       {
           City = city;
       }
       
    }

    public CreateAccommodationPage()
    {
        
    }

  
    public string? AccommodationName { get; set; } 
    public string? Location { get; set; }
   
    public string[]? AccommodationType { get; set; } = new[] {"Apartment", "House", "Cottage" };
    public string? MaxGuests { get; set; }
    public string? MinGuests { get; set; }
    
    public int CurrentType { get; set; } = -1;
    
    public string? CancelationDays { get; set; }
    
    public string? Photos { get; set;  }
    
    public string? City { get; set; }
    
    public string? Country { get; set;  }
   
    public void CreateNewAccommodation(object sender, RoutedEventArgs e)
{
    if (AreAllAccommodationConditionsMet())
    {
        if (!int.TryParse(MaxGuests, out int maxGuests) || maxGuests <= 0)
        {
            MessageBox.Show("Please enter a valid value-number, for Max Guest Number.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (!int.TryParse(MinGuests, out int minGuests) || minGuests <= 0)
        {
            MessageBox.Show("Please enter a valid value-number, for Min Duration.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (!int.TryParse(CancelationDays, out int cancelationDays) || cancelationDays <= 0)
        {
            MessageBox.Show("Please enter a valid value-number, for Cancellation Days.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        Enum.TryParse<AccommodationType>(AccommodationType[CurrentType], out var type);
        
        _currentAccommodation = new Accommodation(AccommodationName, new Location(City, Country), type, maxGuests, minGuests, cancelationDays, _loggedUser.Id);
        _currentAccommodation.Photos = Photos.Split("*").ToList();
        //_currentAccommodation.OwnerId = _loggedUser.Id;
        _accommodationService.Save(_currentAccommodation);
        //  _ownerWindow.MainFrameOwnerWindow.Content = new MyAccommodations();
        MessageBox.Show($"Accommodation has been successfully created", "Accommodation Ceated", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}

private bool AreAllAccommodationConditionsMet()
{
    return !string.IsNullOrEmpty(AccommodationName) && 
           !string.IsNullOrEmpty(City) && 
           !string.IsNullOrEmpty(Country) &&
           !string.IsNullOrEmpty(MaxGuests) && 
           CurrentType != -1 &&
           !string.IsNullOrEmpty(MinGuests) && 
           !string.IsNullOrEmpty(CancelationDays) && 
           CancelationDays != "0" &&
           MinGuests != "0" && 
           MaxGuests != "0";
}


    public void UploadPhotos(object sender, MouseButtonEventArgs e)
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
    private ObservableCollection<string> uploadedFiles = new ObservableCollection<string>();

    public void CancelButton(object sender, RoutedEventArgs e)
    {
        _ownerWindow.MainFrameOwnerWindow.Content = new HomePage();
    }

    private void SetFocusToTextBox(object sender, RoutedEventArgs e)
    {
        TextBox textBox = sender as TextBox;
        if (textBox != null)
        {
            textBox.Focus();
            textBox.SelectAll(); // Opciono selektovanje svih tekstualnih sadržaja unutar TextBox-a
        } 
    }
}

//              _currentAccommodation = new Accommodation(AccommodationName, new Location(Location.Split(", ")[0], Location.Split(", ")[1]),type, (int)MaxGuests,(int)MinGuests,(int)CancelationDays);
