using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;
using BookingApp.WPF.View.TourGuidePages;
using DelegateCommand = BookingApp.WPF.View.OwnerPages.DelegateCommand;

namespace BookingApp.WPF.ViewModel.TourGuide;

public class MyAccountPageViewModel : ViewModelBase
{
    private readonly TourGuideWindow _tourGuideWindow;
    private readonly User _loggedUser;
    private string _username;
    private readonly ITourService _tourService;
    private readonly ITourExecutionService _tourExecutionService;
    private readonly ITourReservationService _tourReservationService;
    private readonly IVoucherService _voucherService;
    private readonly IUserService _userService;
    private readonly ITourGuideService _tourGuideService;
    private readonly ITourReviewService _tourReviewService;

    public string Username
    {
        get => _username;
        set
        {
            if (value != _username)
            {
                _username = value;
                OnPropertyChanged();
            }
        }
    }

    private string _userType;

    public string UserType
    {
        get => _userType;
        set
        {
            if (value != _userType)
            {
                _userType = value;
                OnPropertyChanged();
            }
        }
    }
    
    private string _role;

    public string Role
    {
        get => _role;
        set
        {
            if (value != _role)
            {
                _role = value;
                OnPropertyChanged();
            }
        }
    }

    public MyAccountPageViewModel(TourGuideWindow tourGuideWindow)
    {
        _tourGuideWindow = tourGuideWindow;
        OpenCreateTourPageCommand = new DelegateCommand(OpenCreateTourPage);
        OpenTrackTourPageCommand = new DelegateCommand(OpenTrackTourPage);
        OpenScheduleTourPageCommand = new DelegateCommand(OpenScheduleTourPage);
        OpenReviewsPageCommand = new DelegateCommand(OpenReviewsPage);
        DeleteAccountCommand = new View.DelegateCommand(DeleteAccount);
        _tourService = Injector.Container.Resolve<ITourService>();
        _tourExecutionService = Injector.Container.Resolve<ITourExecutionService>();
        _tourReservationService = Injector.Container.Resolve<ITourReservationService>();
        _voucherService = Injector.Container.Resolve<IVoucherService>();
        _userService = Injector.Container.Resolve<IUserService>();
        _tourGuideService = Injector.Container.Resolve<ITourGuideService>();
        _tourReviewService = Injector.Container.Resolve<ITourReviewService>();

        var app = Application.Current as App;
        _loggedUser = app.LoggedUser;
        InitializeUser();
        InitializeRole();
    }

    public ICommand OpenCreateTourPageCommand { get; private set; }
    public ICommand OpenTrackTourPageCommand { get; private set; }
    public ICommand OpenScheduleTourPageCommand { get; private set; }
    public ICommand OpenReviewsPageCommand { get; private set; }
    public ICommand DeleteAccountCommand { get; private set; }

    private void OpenCreateTourPage(object o)
    {
        _tourGuideWindow.MainFrameTourGuideWindow.Content = new CreateTourPage(_tourGuideWindow);
    }

    private void OpenTrackTourPage(object o)
    {
        _tourGuideWindow.MainFrameTourGuideWindow.Content = new TrackToursPage(_tourGuideWindow);
    }

    private void OpenScheduleTourPage(object o)
    {
        _tourGuideWindow.MainFrameTourGuideWindow.Content = new ScheduleTourPage(_tourGuideWindow);
    }

    private void OpenReviewsPage(object o)
    {
        {
            _tourGuideWindow.MainFrameTourGuideWindow.Content = new TourReviewsPage
            {
                DataContext = new TourReviewsPageViewModel(_tourGuideWindow)
            };
        }
    }

    private void InitializeUser()
    {
        Username = _loggedUser.Username;
        UserType = _loggedUser.Type.ToString();
    }

    private void DeleteAccount(object o)
    {
        MessageBoxResult result = MessageBox.Show("Are you sure you want to delete your account?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            
        if (result == MessageBoxResult.Yes)
        {
            foreach (var tour in _tourService.GetByTourGuideId(_loggedUser.Id))
            {
                foreach (var execution in _tourExecutionService.GetByTourId(tour.Id))
                {
                    foreach (var reservation in _tourReservationService.GetByTourExecutionId(execution.Id))
                    {
                        Voucher voucher = new Voucher();
                        voucher.TouristId = reservation.TouristId;
                        voucher.IsValid = true;
                        voucher.ExpirationDate = DateTime.Now.AddYears(2);
                        voucher.Reason = Reason.GuideQuit;
                        voucher.TourGuideId = _loggedUser.Id;
                        _voucherService.Save(voucher);
                    }

                    _tourExecutionService.Delete(execution);
                }

                _tourService.Delete(tour);
            }

            _userService.Delete(_loggedUser);
            _tourGuideService.Delete(_tourGuideService.GetById(_loggedUser.Id));
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            _tourGuideWindow.Close();
        }
    }

    private bool IsSuperGuide()
    {
        var languages = new Dictionary<Language, int>();
        var languageRatings = new Dictionary<Language, List<double>>();

        foreach (var tour in _tourService.GetByTourGuideId(_loggedUser.Id))
        {
            foreach (var execution in _tourExecutionService.GetByTourId(tour.Id).Where(e => e.StartDate.Year == DateTime.Now.Year))
            {
                var language = _tourService.GetById(_tourService.GetTourIdByExecutionId(execution.Id)).Language;

                if (!languages.ContainsKey(language))
                {
                    languages[language] = 0;
                    languageRatings[language] = new List<double>();
                }

                languages[language] += 1;

                foreach (var review in _tourReviewService.GetByTourExectuionId(execution.Id))
                {
                    double averageRating = (review.GuidesKnowledge + review.GuidesLanguageSkills + review.AmusementLevel) / 3.0;
                    languageRatings[language].Add(averageRating);
                }
            }
        }

        foreach (var language in languages.Keys)
        {
            if (languages[language] >= 20)
            {
                double averageRating = languageRatings[language].Average();
                if (averageRating > 4.0)
                {

                    var guide = _tourGuideService.GetById(_loggedUser.Id);
                    guide.IsSuperGuide = true;
                    _tourGuideService.Update(guide);
                    return true;
                }
            }
        }

        return false;
    }

    private void InitializeRole()
    {
        if (IsSuperGuide())
        {
            Role = "Super Guide";
        }
        else
        {
            Role = "Regular Guide";
        }
    }
    
    

}