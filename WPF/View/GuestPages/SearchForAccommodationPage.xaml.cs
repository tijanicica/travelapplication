// SearchForAccommodationPage.xaml.cs

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;

namespace BookingApp.WPF.View.GuestPages
{
    public partial class SearchForAccommodationPage : Page
    {
        private readonly GuestWindow _guestWindow;
        //private readonly AccommodationController _accommodationController;
        private IAccommodationService _accommodationService;

        public SearchForAccommodationPage()
        {
            
        }

        public SearchForAccommodationPage(GuestWindow guestWindow, IAccommodationService accommodationService)
        {
            InitializeComponent();
            _guestWindow = guestWindow;
            _accommodationService = accommodationService;

        }
        
        private string GetTypeFilter()
        {
            string typeFilter = "";
            if (ApartmentCheckBox.IsChecked == true)
            {
                typeFilter = "Apartment";
            }
            else if (HouseCheckBox.IsChecked == true)
            {
                typeFilter = "House";
            }
            else if (CottageCheckBox.IsChecked == true)
            {
                typeFilter = "Cottage";
            }
            return typeFilter;
        }
        
        private bool IsFilteredAccommodationsEmpty(List<Accommodation> filteredAccommodations)
        {
            return filteredAccommodations == null || filteredAccommodations.Count == 0;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string nameFilter = NameTextBox.Text;
            string locationFilter = LocationTextBox.Text.Replace(" ", "");
            string typeFilter = GetTypeFilter();
            int maxGuestsFilter = int.TryParse(MaxGuestsTextBox.Text, out int maxGuests) ? maxGuests : 0;
            int minDaysFilter = int.TryParse(MinDaysTextBox.Text, out int minDays) ? minDays : 0;
            
            _accommodationService = Injector.Container.Resolve<IAccommodationService>();

            var filteredAccommodations = _accommodationService.FilterAccommodations(nameFilter, locationFilter, typeFilter, maxGuestsFilter, minDaysFilter);

            if (IsFilteredAccommodationsEmpty(filteredAccommodations))
            {
                MessageBox.Show("No accommodations found with the specified filters.");
                return;
            }
            
            SearchCriteria searchCriteria = new SearchCriteria
            {
                Location = locationFilter,
                Name = nameFilter,
                MaxGuests = maxGuestsFilter,
                MinDays = minDaysFilter,
                Type = typeFilter
            };
            var filteredAccommodationsPage = new FilteredAccommodationsPage(filteredAccommodations, _guestWindow, _accommodationService, searchCriteria);
            _guestWindow.MainFrameGuestWindow.Content = filteredAccommodationsPage;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
        
        private void MaskedTextBox_GotKeyboardFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.SelectAll();
            }
        }
    }
    
    public class SearchCriteria
    {
        public string Location { get; set; }
        public string Name { get; set; }
        public int MaxGuests { get; set; }
        public int MinDays { get; set; }
        
        public string Type { get; set; }

    }

}
