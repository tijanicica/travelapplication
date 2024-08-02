using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;

namespace BookingApp.WPF.View.GuestPages
{
    public partial class FilteredAccommodationsPage : Page
    {
        private readonly GuestWindow _guestWindow;
        private IAccommodationService _accommodationService;
        private SearchCriteria searchCriteria;


        public ObservableCollection<Accommodation> FilteredAccommodations { get; set; }

        public FilteredAccommodationsPage(List<Accommodation> filteredAccommodations, GuestWindow guestWindow, IAccommodationService accommodationService, SearchCriteria searchCriteria)
        {
            InitializeComponent();
            _guestWindow = guestWindow;
            _accommodationService = accommodationService;

            FilteredAccommodations = new ObservableCollection<Accommodation>(filteredAccommodations);
            DataContext = this;
            this.searchCriteria = searchCriteria;

            // Generisanje teksta na osnovu modela pretrage
            List<string> searchInfoList = new List<string>();

            if (!string.IsNullOrEmpty(searchCriteria.Location))
            {
                searchInfoList.Add($"located in {searchCriteria.Location}");
            }

            if (searchCriteria.MinDays > 0)
            {
                searchInfoList.Add($"for {searchCriteria.MinDays} days");
            }

            if (searchCriteria.MaxGuests > 0)
            {
                searchInfoList.Add($"with {searchCriteria.MaxGuests} guests");
            }

            string searchInfoText = "You have searched for accommodation";
            if (searchInfoList.Count > 0)
            {
                searchInfoText += " " + string.Join(", ", searchInfoList);
            }
            SearchInfoTextBlock.Text = searchInfoText + ".";

            // Prikazivanje podataka o pretrazi korisniku
          
        }

        private void Details_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                Accommodation accommodation = button.Tag as Accommodation;
                if (accommodation != null && _guestWindow != null && _guestWindow.MainFrameGuestWindow != null)
                {
                    DetailsPage detailsPage = new DetailsPage(accommodation);
                    _guestWindow.MainFrameGuestWindow.Content = detailsPage;
                }
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (_guestWindow != null && _guestWindow.MainFrameGuestWindow != null && _guestWindow.MainFrameGuestWindow.CanGoBack)
            {
                _guestWindow.MainFrameGuestWindow.GoBack();
            }
        }
    }
}
