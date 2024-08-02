using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;

namespace BookingApp.WPF.ViewModel.Owner;

public class SuperOwnerPageViewModel: ViewModelBase
{
    private OwnerWindow _ownerWindow;
    private readonly IAccommodationReviewService _accommodationReviewService;

    private IUserService _userService;
    private User _loggedUser;
    private List<AccommodationReview> _accommodationReviews;

    private readonly IOwnerService _ownerService;
   
    
    private string _username;

    public string UserName
    {
        get => _username;
        set {
            if (value != _username)
            {
                _username = value;
                OnPropertyChanged();
            }}
    }

    private string _superowner;
    
    private bool _isSuperOwner;

    public bool IsSuperOwner
    {
        get => _isSuperOwner;
        set
        {
            if (value != _isSuperOwner)
            {
                _isSuperOwner = value;
                OnPropertyChanged();
            }
        }
    }

    public string SuperOwner
    {
        get => _superowner;
        set {
            if (value != _superowner)
            {
                _superowner = value;
                OnPropertyChanged();
            }}
    }

    public SuperOwnerPageViewModel(OwnerWindow ownerWindow)
    {
        _ownerWindow = ownerWindow;
        var app = Application.Current as App;
        _accommodationReviewService = Injector.Container.Resolve<IAccommodationReviewService>();
        _userService = Injector.Container.Resolve<IUserService>();
        _ownerService = Injector.Container.Resolve<IOwnerService>();

        _loggedUser = app.LoggedUser; // Dobijanje ulogovanog korisnika

        InitializeAccommodationReviews();
        UserName = _loggedUser.Username;
        IsSuperOwner = (_loggedUser as Domain.Model.Owner)?.IsSuperOwner ?? false; 

    }

    private void InitializeAccommodationReviews()
    {
        // Učitavanje ocena smeštaja
        _accommodationReviews = _accommodationReviewService.GetByOwnerId(_loggedUser.Id).ToList();

        // Provera statusa super-vlasnika
        CheckSuperOwnerStatus();
        
    }

    private void CheckSuperOwnerStatus()
    {
        double totalRating = 0;
        foreach (var review in _accommodationReviews)
        {
            totalRating += (review.AccommodationCleanliness + review.OwnerCorrectness) / 2.0;
        }
        double averageRating = totalRating / _accommodationReviews.Count;

        if (averageRating >= 4.5 && _accommodationReviews.Count >= 50)
        {
            // Postavljanje statusa super-vlasnika
            SuperOwner = "Yes";
            IsSuperOwner = true;

            
            // Dodati logiku za posebno isticanje super-vlasnika
        }
        else

        {
            IsSuperOwner = false;
            SuperOwner = "No";
        }
        
        _ownerService.UpdateIsSuperOwner(_loggedUser.Id, IsSuperOwner);

    }

}