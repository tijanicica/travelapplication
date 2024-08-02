using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Dto;
using BookingApp.Utils;

namespace BookingApp.WPF.View.OwnerPages;


public partial class FilteredGuestsPage: Page, INotifyPropertyChanged
{
    private OwnerWindow _ownerWindow;
   // private GuestController _guestController;
    private readonly IGuestService _guestService;
    private User _loggedUser;
    private Guest _guest;
    //private AccommodationController _accommodationController;
    private readonly IAccommodationService _accommodationService;
   // private AccommodationReservationController _accommodationReservationController;
    private readonly IAccommodationReservationService _accommodationReservationService;
    private Accommodation _accommodation;
    private ObservableCollection<GuestDto> _allGuestsDtoList;



    public FilteredGuestsPage()
    {
        InitializeComponent();
        this.DataContext = this;

        var app = Application.Current as App;
        _guestService = Injector.Container.Resolve<IGuestService>();
        _accommodationService = Injector.Container.Resolve<IAccommodationService>();
        _accommodationReservationService = Injector.Container.Resolve<IAccommodationReservationService>();
        _loggedUser = app.LoggedUser;

        _allGuestsDtoList = _accommodationReservationService.GetGuestsForReview(_loggedUser.Id);

           

    }
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    

    public void ReviewButton_Click(object sender, RoutedEventArgs e)
    {
        Button reserveButton = sender as Button;
        GuestDto selectedGuest = reserveButton.DataContext as GuestDto;

        if (selectedGuest != null)
        {
            this.NavigationService?.Navigate(new GuestReviewPage(selectedGuest.Id));
        }
        
    }
    public ObservableCollection<GuestDto> AllGuestsDto
    {
        get { return _allGuestsDtoList; }
        set
        {
           if (value != _allGuestsDtoList)
            {
                _allGuestsDtoList = value;
                OnPropertyChanged("AllGuestsDto");
           }
        }                                       
    }

}