using System;
using System.Windows;
using System.Windows.Input;
using BookingApp.Domain.Model;
using BookingApp.WPF.View.TourGuidePages;
using BookingApp.WPF.ViewModel.TourGuide;

namespace BookingApp.WPF.View;

public partial class TourGuideWindow : Window
{
    private User _loggedUser;
    public string Username
    {
        get { return _loggedUser?.Username; } 
        set { } 
    }
    public TourGuideWindow()
    {
        InitializeComponent();
        MainFrameTourGuideWindow.Content = new HomePage(this);
        this.DataContext = this;
        var app = Application.Current as App;
        _loggedUser = app.LoggedUser;
        Username = _loggedUser.Username;
    }

    private void ToggleMenuPopup(object sender, MouseButtonEventArgs e)
    {
        MenuPopup.IsOpen = !MenuPopup.IsOpen;
    }

    private void CloseMenuPopup(object sender, RoutedEventArgs e)
    {
        MenuPopup.IsOpen = false;
    }

    private void MenuPopup_Opened(object? sender, EventArgs e)
    {
        MenuButton.Visibility = Visibility.Collapsed;
    }

    private void MenuPopup_Closed(object? sender, EventArgs e)
    {
        MenuButton.Visibility = Visibility.Visible;
    }

    private void OpenHomePage(object sender, RoutedEventArgs e)
    {
        MainFrameTourGuideWindow.Content = new HomePage(this);
        MenuPopup.IsOpen = false;

    }

    private void OpenCreateTourPage(object sender, RoutedEventArgs e)
    {
        MainFrameTourGuideWindow.Content = new CreateTourPage(this);
        MenuPopup.IsOpen = false;

    }

    private void OpenScheduleTourPage(object sender, RoutedEventArgs e)
    {
        MainFrameTourGuideWindow.Content = new ScheduleTourPage(this);
        MenuPopup.IsOpen = false;

    }

    private void OpenTrackTourPage(object sender, RoutedEventArgs e)
    {
        MainFrameTourGuideWindow.Content = new TrackToursPage(this);
        MenuPopup.IsOpen = false;
        
    }

    private void Logout(object sender, RoutedEventArgs e)
    {
        MessageBoxResult result = MessageBox.Show("Are you sure you want to log out?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Close();
        }
    }

    private void OpetCancelTourPage(object sender, RoutedEventArgs e)
    {
        MainFrameTourGuideWindow.Content = new CancelTourPage
        {
            DataContext = new CancelTourPageViewModel(this)
        };
        MenuPopup.IsOpen = false;
    }

    private void OpenTourReviewsPage(object sender, RoutedEventArgs e)
    {
        MainFrameTourGuideWindow.Content = new TourReviewsPage {
            DataContext = new TourReviewsPageViewModel(this)
        };
        MenuPopup.IsOpen = false;
    }

    private void OpenStatisticsMainPage(object sender, RoutedEventArgs e)
    {
        MainFrameTourGuideWindow.Content = new StatisticsMainPage() {
            DataContext = new StatisticsMainPageViewModel(this)
        };
        MenuPopup.IsOpen = false;
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        if (MainFrameTourGuideWindow.CanGoBack)
        {
            MainFrameTourGuideWindow.GoBack(); 
        }
    }

    private void ForwardButton_Click(object sender, MouseButtonEventArgs e)
    {
        if (MainFrameTourGuideWindow.NavigationService.CanGoForward)
        {
            MainFrameTourGuideWindow.NavigationService.GoForward();
        }
    }


    private void OpenAllRequestsPage(object sender, RoutedEventArgs e)
    {
        MainFrameTourGuideWindow.Content = new AllRequestsPage() {
            DataContext = new AllRequestsPageViewModel(this)
        };
        MenuPopup.IsOpen = false;
    }

    private void OpenMyAccountPage(object sender, RoutedEventArgs e)
    {
        MainFrameTourGuideWindow.Content = new MyAccountPage() {
            DataContext = new MyAccountPageViewModel(this)
        };
        MenuPopup.IsOpen = false;
    }
    
    private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (Keyboard.Modifiers == ModifierKeys.Control)
        {
            switch (e.Key)
            {
                case Key.H:
                    OpenHomePage(null, null);
                    break;
                case Key.B:
                    if (MainFrameTourGuideWindow.CanGoBack)
                        MainFrameTourGuideWindow.GoBack();
                    break;
                case Key.F:
                    if (MainFrameTourGuideWindow.NavigationService.CanGoForward)
                        MainFrameTourGuideWindow.NavigationService.GoForward();
                    break;
            }
        }
    }
}