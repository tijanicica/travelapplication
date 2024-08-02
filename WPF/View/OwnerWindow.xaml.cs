using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BookingApp.Domain.Model;
using BookingApp.Dto;
using BookingApp.WPF.View.OwnerPages;
using BookingApp.WPF.ViewModel.Owner;
using OwnerNotification = BookingApp.WPF.View.OwnerPages.OwnerNotification;

namespace BookingApp.WPF.View;

public partial class OwnerWindow : Window
{
    private User _loggedUser;
    public string Username
    {
        get { return _loggedUser?.Username; } 
        set { } 
    }

    public OwnerWindow()
    {
        InitializeComponent();
        MainFrameOwnerWindow.Content = new MyAccommodations()
        {
            DataContext = new MyAccommodationPageViewModel(this)
        };
        var app = Application.Current as App;
        _loggedUser = app.LoggedUser;
        Username = _loggedUser.Username;
    }

    private void OpenHomePage(object sender, RoutedEventArgs e)
    {
        MainFrameOwnerWindow.Content = new HomePage();
    }

    private void OpenCreateAccommodationPage(object sender, RoutedEventArgs e)
    {
        MainFrameOwnerWindow.Content = new CreateAccommodationPage(this);
    }

    private void OpenGuestReviewPage(object sender, RoutedEventArgs e)
    {
        MainFrameOwnerWindow.Content = new FilteredGuestsPage();
    }

    private void AccomodationReview(object sender, RoutedEventArgs e)
    {
        MainFrameOwnerWindow.Content = new AccommodationReviews()
        {
            DataContext = new AccommodationReviewsPageViewModel(this)
        };
    }

    private void ReservationOverview(object sender, RoutedEventArgs e)
    {
        MainFrameOwnerWindow.Content = new ReservationOverview()
        {
            DataContext = new ReservationOverviewPageViewModel(this)
        };
    }

    private void MyAccount(object sender, MouseButtonEventArgs e)
    {
        MainFrameOwnerWindow.Content = new MyAccount()
        {
            DataContext = new SuperOwnerPageViewModel(this)
        };
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

    private void HelpButton(object sender, RoutedEventArgs e)
    {
        string helpMessage = string.Empty;

        // Provera koji prozor je trenutno aktivan
        if (MainFrameOwnerWindow.Content.GetType() == typeof(HomePage))
        {
            helpMessage = "Ovo je pomoć za početnu stranicu.";
        }
        else if (MainFrameOwnerWindow.Content.GetType() == typeof(CreateAccommodationPage))
        {
            helpMessage = "To register new accommodation, you need to fill in the following fields. Once you have filled them out, click on the \"Next\" button to upload pictures of your accommodation. By clicking the 'Cancel' button, you will return to the homepage.Enter->OK\nOK";
        }
        else if (MainFrameOwnerWindow.Content.GetType() == typeof(GuestReviewPage))
        {
            helpMessage = "Ratings range from 1 to 5, with 5 being the best and 1 being the worst. By pressing the 'Submit' button, you will finish rating the guest.By clicking the 'Cancel' button, you will return to the homepage. Enter->OK\nOK";
        }
        else if (MainFrameOwnerWindow.Content.GetType() == typeof(RenovationPage))
        {
            helpMessage = "The owner chooses the accommodation, specifies the renovation dates (start and end), and inputs the estimated renovation duration. Guests cannot book the accommodation during the renovation period.By pressing the 'Submit' button, you will complete plan renovation.Enter->OK\nOK";
        }
        else if (MainFrameOwnerWindow.Content.GetType() == typeof(StatisticsYearPage))
        {
            helpMessage = "The owner selects the accommodation and views statistics by years. If they pick a specific year, they see data by months. It also shows when the accommodation was most occupied.Enter->OK\nOK";
        }
        else if (MainFrameOwnerWindow.Content.GetType() == typeof(ReservationOverview))
        {
            helpMessage = "\nIf you want to reject, click on the 'Reject' button and add a comment if desired. If you want to approve, click on the 'Approve' button Enter->OK\nOK";
        }
        else if (MainFrameOwnerWindow.Content.GetType() == typeof(RenovationPage))
        {
            helpMessage = "The owner chooses the accommodation, specifies the renovation dates (start and end), and inputs the estimated renovation duration. Guests cannot book the accommodation during the renovation period.By pressing the 'Submit' button, you will complete plan renovation. Enter->OK\nOK";
        }
        else if (MainFrameOwnerWindow.Content.GetType() == typeof(SelectedForum))
        {
            helpMessage = "You can report a comment by clicking the \"Report\" button, and you can create your own comment by clicking the \"Write\" button. The \"Close Forum\" button is used to follow the forum on the homepage. Enter->OK\nOK";
        }
        else
        {
            helpMessage = "Nema dostupne pomoći za ovaj deo aplikacije.Enter->OK\nOK";
        }

        // Prikazivanje odgovarajuće pomoći
        MessageBox.Show(helpMessage, "Pomoć", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void MyAccomodations(object sender, RoutedEventArgs e)
    {
        MainFrameOwnerWindow.Content = new MyAccommodations()
        {
            DataContext = new MyAccommodationPageViewModel(this)
        };
    }

    private void AllRenovation(object sender, RoutedEventArgs e)
    {
        MainFrameOwnerWindow.Content = new AllRenovations()
        {
            DataContext = new AllRenovationsPageViewModel(this)
        };
    }

    private void AllForums(object sender, RoutedEventArgs e)
    {
        MainFrameOwnerWindow.Content = new AllForums()
        {
            DataContext = new AllForumPageViewModel(this, _loggedUser)
        };
    }

    private void Notification(object sender, RoutedEventArgs e)
    {
        MainFrameOwnerWindow.Content = new OwnerNotification()
        {
            DataContext = new OwnerNotificationPageViewModel(this, _loggedUser)
        };
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (Keyboard.Modifiers == ModifierKeys.Control)
        {
            var currentPage = MainFrameOwnerWindow.Content as Page;

            if (currentPage is MyAccommodations myAccommodationsPage)
            {
                var dataGrid = FindVisualChild<DataGrid>(myAccommodationsPage);
                var selectedAccommodation = dataGrid?.SelectedItem as MyAccommodationModelViewModel;

                if (selectedAccommodation != null)
                {
                    var viewModel = myAccommodationsPage.DataContext as MyAccommodationPageViewModel;
                    switch (e.Key)
                    {
                        case Key.V:
                            viewModel?.StatisticsCommand.Execute(selectedAccommodation.Id);
                            break;
                        case Key.P:
                            viewModel?.RenovationCommand.Execute(selectedAccommodation.Id);
                            break;
                    }
                }
            }
            else if (currentPage is AccommodationReviews accommodationReviewsPage)
            {
                var dataGrid = FindVisualChild<DataGrid>(accommodationReviewsPage);
                var selectedReview = dataGrid?.SelectedItem as AccomodationReviewModelViewModel;

                if (selectedReview != null)
                {
                    var viewModel = accommodationReviewsPage.DataContext as AccommodationReviewsPageViewModel;
                    switch (e.Key)
                    {
                        case Key.V:
                            viewModel?.ViewCommand.Execute(selectedReview.ratingID);
                            break;
                    }
                }
            }
            else if (currentPage is OneAccommodationReviewPagee oneAccommodationReviewPage)
            {
                var viewModel = oneAccommodationReviewPage.DataContext as OneAccommodationReviewPageViewModel;
                if (viewModel != null && e.Key == Key.C)
                {
                    viewModel.CancelCommand.Execute(null);
                }
            }
            else if (currentPage is CreateAccommodationPage createAccommodationPage)
            {
                if (e.Key == Key.C)
                {
                    createAccommodationPage.CancelButton(null, null);
                }
                else if (e.Key == Key.S)
                {
                    createAccommodationPage.CreateNewAccommodation(null, null);
                }
                else if (e.Key == Key.P)
                {
                    createAccommodationPage.UploadPhotos(null, null);
                }
            }
            else if (currentPage is StatisticsYearPage statisticsYearPage)
            {
                var dataGrid = FindVisualChild<DataGrid>(statisticsYearPage);
                var selectedStat = dataGrid?.SelectedItem as StatisticsAccommodation;

                if (selectedStat != null)
                {
                    var viewModel = statisticsYearPage.DataContext as StatisticsYearPageViewModel;
                    if (e.Key == Key.D)
                    {
                        viewModel?.DetaileStatsCommand.Execute(selectedStat);
                    }
                }
            }
            else if (currentPage is RenovationPage renovationPage)
            {
                var dataGrid = FindVisualChild<DataGrid>(renovationPage);
                var selectedDatePair = dataGrid?.SelectedItem as Tuple<DateTime, DateTime>;

                var viewModel = renovationPage.DataContext as RenovationPageViewModel;
                switch (e.Key)
                {
                    case Key.S:
                        if (selectedDatePair != null)
                        {
                            viewModel?.SubmitCommand.Execute(selectedDatePair);
                        }
                        break;
                    case Key.F:
                        viewModel?.CheckAvailableCommand.Execute(null);
                        break;
                    case Key.C:
                        viewModel?.CancelCommand.Execute(null);
                        break;
                }
            }
            else if (currentPage is FilteredGuestsPage filteredGuestsPage)
            {
                var dataGrid = FindVisualChild<DataGrid>(filteredGuestsPage);
                var selectedGuest = dataGrid?.SelectedItem as GuestDto;

                // Proveravamo da li je pritisnut taster 'D' uz istovremeno pritiskanje Ctrl tastera
                if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.D)
                {
                    Button reviewButton = new Button(); // Pravimo novi Button
                    reviewButton.DataContext = selectedGuest; // Postavljamo DataContext na izabrani gost
                    filteredGuestsPage.ReviewButton_Click(reviewButton, null); // Pozivamo ReviewButton_Click metod
                }
            }
            else  if (currentPage is GuestReviewPage guestReviewPage)
            {
                switch (e.Key)
                {
                    case Key.C:
                        guestReviewPage.CancelButton_Click(null, null);
                        break;
                    case Key.S:
                        guestReviewPage.SubmitButton_Click(null, null);
                        break;
                    default:
                        break;
                }
            }
            else if (currentPage is ReservationOverview reservationOverviewPage)
            {
                var dataGrid = FindVisualChild<DataGrid>(reservationOverviewPage);
                var selectedReservation = dataGrid?.SelectedItem as ReservationRescheduleDto;

                if (selectedReservation != null)
                {
                    var viewModel = reservationOverviewPage.DataContext as ReservationOverviewPageViewModel;
                    switch (e.Key)
                    {
                        case Key.R:
                            viewModel?.RejectCommand.Execute(selectedReservation.Id);
                            break;
                        case Key.W:
                            viewModel?.ApproveCommand.Execute(selectedReservation.Id);
                            break;
                    }
                }
            }
            else if (currentPage is AllForums allForumsPage)
            {
                var dataGrid = FindVisualChild<DataGrid>(allForumsPage);
                var selectedForum = dataGrid?.SelectedItem as Forum;

                if (selectedForum != null)
                {
                    var viewModel = allForumsPage.DataContext as AllForumPageViewModel;
                    switch (e.Key)
                    {
                        case Key.V:
                            viewModel?.ViewSelectedForumCommand.Execute(selectedForum);
                            break;
                    }
                }
            }
            switch (e.Key)
            {
                case Key.D1:
                    OpenHomePage(this, null);
                    break;
                case Key.D2:
                    OpenCreateAccommodationPage(this, null);
                    break;
                case Key.D3:
                    MyAccomodations(this, null);
                    break;
                case Key.D4:
                    OpenGuestReviewPage(this, null);
                    break;
                case Key.D5:
                    AccomodationReview(this, null);
                    break;
                case Key.D6:
                    AllRenovation(this, null);
                    break;
                case Key.D7:
                    Notification(this, null);
                    break;
                case Key.D8:
                    ReservationOverview(this, null);
                    break;
                case Key.D9:
                    AllForums(this, null);
                    break;
                case Key.H:
                    HelpButton(this, null);
                    break;
                default:
                    break;
            }
        }
    }
    private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            if (child != null && child is T)
                return (T)child;

            var childOfChild = FindVisualChild<T>(child);
            if (childOfChild != null)
                return childOfChild;
        }
        return null;
    }
}
