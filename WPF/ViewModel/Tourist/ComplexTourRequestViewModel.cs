using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;
using BookingApp.WPF.View.TouristPages;
using DelegateCommand = BookingApp.WPF.View.OwnerPages.DelegateCommand;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class ComplexTourRequestViewModel : ViewModelBase
    {
        private User _loggedUser;
        private int _regularToursNumber;
        private IComplexTourPartService _complexTourPartService;
        private IComplexTourRequestService _complexTourRequestService;
        private TouristWindow _touristWindow;

        public ICommand SubmitTourRequestCommand { get; private set; }
        public ICommand CancelTourRequestCommand { get; private set; }

        public ObservableCollection<ComplexTourPartViewModel> ComplexTourParts { get; }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public int RegularToursNumber
        {
            get => _regularToursNumber;
            set
            {
                if (_regularToursNumber != value)
                {
                    if (value >= 1)
                    {
                        _regularToursNumber = value;
                        OnPropertyChanged();
                        UpdateRegularTourRequests();
                        ErrorMessage = string.Empty; // Clear error message
                    }
                    else
                    {
                        ErrorMessage = "Number must be at least 1.";
                    }
                    OnPropertyChanged(nameof(ErrorMessage));
                }
            }
        }

        public bool IsFormValid => ComplexTourParts.All(part => part.IsValid);
        public ComplexTourRequestViewModel(TouristWindow touristWindow)
        {
            var app = Application.Current as App;
            _loggedUser = app.LoggedUser; 
            _touristWindow = touristWindow;
            _complexTourPartService = Injector.Container.Resolve<IComplexTourPartService>();
            _complexTourRequestService = Injector.Container.Resolve<IComplexTourRequestService>();
            
            ComplexTourParts = new ObservableCollection<ComplexTourPartViewModel>();
            RegularToursNumber = 1;
            
            // Initialize commands
            SubmitTourRequestCommand = new DelegateCommand(SubmitTourRequest);
            CancelTourRequestCommand = new DelegateCommand(CancelTourRequest);
            ComplexTourParts.CollectionChanged += (s, e) => OnPropertyChanged(nameof(IsFormValid));
        }

     /*   private void UpdateRegularTourRequests()
        {
            ComplexTourParts.Clear();
            for (int i = 0; i < RegularToursNumber; i++)
            {
                ComplexTourParts.Add(new ComplexTourPartViewModel());
            }
        }
*/
     private void UpdateRegularTourRequests()
     {
         ComplexTourParts.Clear();
         for (int i = 0; i < RegularToursNumber; i++)
         {
             var part = new ComplexTourPartViewModel();
             part.PropertyChanged += (s, e) =>
             {
                 if (e.PropertyName == nameof(ComplexTourPartViewModel.IsValid))
                 {
                     OnPropertyChanged(nameof(IsFormValid));
                 }
             };
             ComplexTourParts.Add(part);
         }
     }

        private void SubmitTourRequest(object parameter)
        {
            var complexTourRequest = new ComplexTourRequest
            {
                tourParts = new List<int>(),
                Status = ComplexTourRequestStatus.Pending,
                TouristId = _loggedUser.Id
            };

            // Save each regular tour part
            foreach (var complexTour in ComplexTourParts)
            {
                var complexTourPart = new ComplexTourPart
                {
                    Location = complexTour.Location,
                    Description = complexTour.Description,
                    Language = complexTour.Language,
                    PeopleOnTour = new List<PersonOnTour>(complexTour.PeopleOnTour.Select(p => new PersonOnTour 
                    {
                        Name = p.Name,
                        Surename = p.Surname,
                        Age = p.Age
                    })),
                    TouristId = _loggedUser.Id,
                    BeginDate = complexTour.BeginDate,
                    EndDate = complexTour.EndDate,
                    TourRequestDate = DateTime.Now,
                    Status = ComplexTourPartStatus.Pending,
                    TourGuideId = -1, // Assuming initial value, update as needed
                    AcceptedDate = DateTime.MinValue // Initial value, update when accepted
                };

                _complexTourPartService.Save(complexTourPart);
                complexTourRequest.tourParts.Add(complexTourPart.Id); // Assuming the ID is set after saving
            }

            _complexTourRequestService.Save(complexTourRequest);
            Application.Current.Dispatcher.Invoke(() =>
            {
                HomePage homePage = new HomePage
                {
                    MessageText = "Successful tour request!",
                    MessageVisibility = Visibility.Visible
                };

                _touristWindow.MainFrame.NavigationService.Navigate(homePage);
            });
        }

        private void CancelTourRequest(object parameter)
        {
            HomePage homePage = new HomePage();
            // Navigira se do HomePage
            _touristWindow.MainFrame.NavigationService.Navigate(homePage);
        }
    }
}
