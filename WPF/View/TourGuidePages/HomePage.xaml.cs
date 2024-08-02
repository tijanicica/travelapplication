using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using BookingApp.WPF.ViewModel.TourGuide;

namespace BookingApp.WPF.View.TourGuidePages;

public partial class HomePage : Page
{
    private TourGuideWindow _tourGuideWindow;
    private List<string> _imagePaths;
    private int currentIndex = 0;
    private DispatcherTimer timer;
    public HomePage(TourGuideWindow tourGuideWindow)
    {
        InitializeComponent();
        _tourGuideWindow = tourGuideWindow;
        this.DataContext = this;
        _imagePaths = new List<string>
        {
            "pack://application:,,,/Resources/Images/hiking.jpg",
            "pack://application:,,,/Resources/Images/hiking.jpg", 
            "pack://application:,,,/Resources/Images/guide.jpg", 
            "pack://application:,,,/Resources/Images/how_to_take_great_travel_photos.jpg", 
            "pack://application:,,,/Resources/Images/Image-1-Taman-Negara-1.jpg", 
            "pack://application:,,,/Resources/Images/italy.jpg", 
            "pack://application:,,,/Resources/Images/polarlights.jpg", 
            "pack://application:,,,/Resources/Images/prettytravel.jpg", 
            "pack://application:,,,/Resources/Images/Sightseeing-in-Mauritius-02.jpg", 
            "pack://application:,,,/Resources/Images/tourists.jpg", 
            "pack://application:,,,/Resources/Images/where-to-go-on-holiday.jpg", 
            "pack://application:,,,/Resources/Images/sightseeing-m.jpg" 
            
            
        };
        
        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(3); // Change interval as needed
        timer.Tick += Timer_Tick;
        timer.Start();

        // Display the first image
        ShowImage();
        
        

    }
    
    private void Timer_Tick(object sender, EventArgs e)
    {
        // Switch to the next image
        currentIndex = (currentIndex + 1) % _imagePaths.Count;
        ShowImage();
    }

    private void ShowImage()
    {
        // Load the image from the current path
        string imagePath = _imagePaths[currentIndex];
        BitmapImage bitmap = new BitmapImage();
        bitmap.BeginInit();
        bitmap.UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute);
        bitmap.EndInit();

        // Display the image in the Image control
        //SlideshowImage1.Source = bitmap;
        /*SlideshowImage2.Source = bitmap;
        SlideshowImage3.Source = bitmap;
        SlideshowImage4.Source = bitmap;
        SlideshowImage5.Source = bitmap;
        SlideshowImage6.Source = bitmap;*/
    }


    private void OpenTrackTourPage(object sender, RoutedEventArgs e)
    {
        _tourGuideWindow.MainFrameTourGuideWindow.Content = new TrackToursPage(_tourGuideWindow);
    }

    private void OpenCreateTourPage(object sender, RoutedEventArgs e)
    {
        _tourGuideWindow.MainFrameTourGuideWindow.Content = new CreateTourPage(_tourGuideWindow);

    }

    private void ScheduleTourPage(object sender, RoutedEventArgs e)
    {
        _tourGuideWindow.MainFrameTourGuideWindow.Content = new ScheduleTourPage(_tourGuideWindow);
    }
    
    private void OpenTourReviewsPage(object sender, RoutedEventArgs e)
    {
        _tourGuideWindow.MainFrameTourGuideWindow.Content = new TourReviewsPage
       {
            DataContext = new TourReviewsPageViewModel(_tourGuideWindow)
        };
       
    }
    
    
}