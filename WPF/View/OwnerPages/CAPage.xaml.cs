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
public partial class CAPage : Page
{
    private OwnerWindow _ownerWindow ;
    private Accommodation _currentAccommodation;
    private User _loggedUser;
   // private AccommodationController _accommodationController; 
    private  readonly IAccommodationService _accommodationService;
    public CAPage( string? city = null, string? type = null)
    {
        InitializeComponent();
        this.DataContext = this;
        var app = Application.Current as App;
        _loggedUser = app.LoggedUser;
        _accommodationService =  Injector.Container.Resolve<IAccommodationService>();
        UploadedFilesListBox.ItemsSource = uploadedFiles;
       // _ownerWindow = ownerWindow;
       if (city != null)
       {
           City = city;
       }
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
      
    }
    

  
    public string? AccommodationName { get; set; }
    public string? Location { get; set; }
   
    public string[]? AccommodationType { get; set; } = new[] {"Apartment", "House", "Cottage" };
    public int? MaxGuests { get; set; }
    public int? MinGuests { get; set; }
    

    
    public int? CancelationDays { get; set; }
    
    public string? Photos { get; set;  }
    
    public string? City { get; set; }
    
    public string? Country { get; set;  }
    public int CurrentType { get; set; } = -1;
   
    private void CreateNewAccommodation(object sender, RoutedEventArgs e)
    {
        if (AreAllAccommodationConditionsMet())
        {
            Enum.TryParse<AccommodationType>(AccommodationType[CurrentType], out var type);
            
                _currentAccommodation = new Accommodation(AccommodationName, new Location(City, Country), type, (int)MaxGuests, (int)MinGuests, (int)CancelationDays, _loggedUser.Id);
                _currentAccommodation.Photos = Photos.Split("*").ToList();
                //_currentAccommodation.OwnerId = _loggedUser.Id;
                _accommodationService.Save(_currentAccommodation);
                this.NavigationService?.Navigate(new MyAccommodations());
          
        }
    }

    private bool AreAllAccommodationConditionsMet()
    {
        return (AccommodationName != null && City != null && Country != null && AccommodationType != null &&
                MaxGuests != null && MinGuests != null && CurrentType != -1 && CancelationDays != null);
    }

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
    private ObservableCollection<string> uploadedFiles = new ObservableCollection<string>();
}