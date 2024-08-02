using System.Collections.ObjectModel;
using System.Windows;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;

namespace BookingApp.WPF.ViewModel.Tourist;

public class NewToursViewModel : ViewModelBase
{
    private User _loggedUser;
    private ITourRequestService _tourRequestService;
    private IUserService _userService;
    private TouristWindow _touristWindow;
    private IRequestedTourService _requestedTourService;
    public ObservableCollection<NewToursModelViewModel> RegularTours { get; set; }
    
    /*
     * ture imaju lokaciju ili jezik - neki zahtev turiste koji nikada nije bio ispunjen
     * (npr. mnogo turista je pravilo zahteve za ture na norveškom jeziku;
     * vodiči su napravili nove ture spram ovih zahteva; biće obavešteni turisti koji
     * među svojim neispunjenim zahtevima imaju one koji imaju jezik norveški). 
     */
    public NewToursViewModel(TouristWindow touristWindow)
    {
        var app = Application.Current as App;
        _touristWindow = touristWindow;
        _loggedUser = app.LoggedUser; 
        _tourRequestService = Injector.Container.Resolve<ITourRequestService>();
        _userService = Injector.Container.Resolve<IUserService>();
        _requestedTourService = Injector.Container.Resolve<IRequestedTourService>();

        InitializeRegularTours();

    }
    
    private void InitializeRegularTours()
    {
        RegularTours = new ObservableCollection<NewToursModelViewModel>();
        
        foreach (var tour in _tourRequestService.GetAllNewTours(_loggedUser.Id)) //kroz sve ture koje su requested, ja nisam touristid, ali ima od mojih requestova lokacija i jezik
        {
            string tourGuideName = _userService.GetUsernameById(_requestedTourService.GetByTourRequestId(tour.Id).TourGuideId);
            
            RegularTours.Add(new NewToursModelViewModel(
                    _requestedTourService.GetByTourRequestId(tour.Id).Date,
                    tourGuideName,
                    tour.Location.Country.ToString() + ", " + tour.Location.City.ToString(),
                    tour.Language.ToString(),
                    tour.Description
                )
            );
        }
    }
}