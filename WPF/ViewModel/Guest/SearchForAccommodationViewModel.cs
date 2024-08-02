using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;
using BookingApp.WPF.View.GuestPages;
using DelegateCommand = BookingApp.WPF.View.OwnerPages.DelegateCommand;

namespace BookingApp.WPF.ViewModel.GuestPages
{
    public class SearchForAccommodationViewModel : ViewModelBase
    {
        private readonly GuestWindow _guestWindow;
        private readonly IAccommodationService _accommodationService;

        public string NameFilter { get; set; }
        public string LocationFilter { get; set; }
        public bool ApartmentFilter { get; set; }
        public bool HouseFilter { get; set; }
        public bool CottageFilter { get; set; }
        public int MaxGuestsFilter { get; set; }
        public int MinDaysFilter { get; set; }

        public ICommand SearchCommand { get; private set; }
        public ICommand BackCommand { get; private set; }

        public SearchForAccommodationViewModel(List<AccommodationReservation> availableReservations, GuestWindow guestWindow)
        {
            _guestWindow = guestWindow;
            _accommodationService = Injector.Container.Resolve<IAccommodationService>();

            //SearchCommand = new DelegateCommand(ExecuteSearchCommand);
            BackCommand = new RelayCommand(ExecuteBackCommand);
        }

        /*private void ExecuteSearchCommand(object parameter)
        {
            var filteredAccommodations = _accommodationService.FilterAccommodations(NameFilter, LocationFilter, GetTypeFilter(), MaxGuestsFilter, MinDaysFilter);

            if (IsFilteredAccommodationsEmpty(filteredAccommodations))
            {
                MessageBox.Show("No accommodations found with the specified filters.");
                return;
            }

            var filteredAccommodationsPage = new FilteredAccommodationsPage(filteredAccommodations, _guestWindow, _accommodationService);
            _guestWindow.MainFrameGuestWindow.Content = filteredAccommodationsPage;
        }*/

        private void ExecuteBackCommand(object parameter)
        {
            // Implementacija logike za povratak na prethodnu stranicu
        }

        private string GetTypeFilter()
        {
            if (ApartmentFilter)
            {
                return "Apartment";
            }
            else if (HouseFilter)
            {
                return "House";
            }
            else if (CottageFilter)
            {
                return "Cottage";
            }

            return "";
        }

        private bool IsFilteredAccommodationsEmpty(List<Accommodation> filteredAccommodations)
        {
            return filteredAccommodations == null || filteredAccommodations.Count == 0;
        }
    }
}
