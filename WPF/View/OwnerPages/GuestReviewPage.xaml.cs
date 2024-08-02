using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;

namespace BookingApp.WPF.View.OwnerPages
{
    public partial class GuestReviewPage : Page
    {
        private OwnerWindow _ownerWindow;
        private User _loggedUser;
        private readonly IGuestReviewService _guestReviewService;
        private readonly IAccommodationReservationService _reservationService;
        private readonly IGuestService _guestService;
        private int _numberOfPeople;
        private int _currentGuestId;

        public AccommodationReservation reservation { get; set; }

        public GuestReviewPage(int currentGuestId)
        {
            InitializeComponent();
            _currentGuestId = currentGuestId;
            
            var app = Application.Current as App;
            _guestService = Injector.Container.Resolve<IGuestService>();
            _guestReviewService = Injector.Container.Resolve<IGuestReviewService>();
            _reservationService = Injector.Container.Resolve<IAccommodationReservationService>();
            _loggedUser = app.LoggedUser;
        }

        public void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ResetControls();
            this.NavigationService?.Navigate(new HomePage());
        }

        public void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            int cleanlinessRating = GetSelectedRadioButtonValue(cleanlinessRatingUniformGrid);
            int ruleFollowingRating = GetSelectedRadioButtonValue(ruleFollowingRatingUniformGrid);
            string comment = commentTextBox.Text;

            GuestReview guestReview = new GuestReview();
            guestReview.GuestId = _currentGuestId;
            guestReview.Cleanliness = cleanlinessRating;
            guestReview.RuleFollowing = ruleFollowingRating;
            guestReview.Comment = comment;
            guestReview.OwnerId = _loggedUser.Id;
            
            _guestReviewService.CreateReview(guestReview);
            ResetControls();
            MessageBox.Show("Thank you for completing review!");
        }
        private int GetSelectedRadioButtonValue(UniformGrid uniformGrid)
        {
            foreach (var child in uniformGrid.Children)
            {
                if (child is RadioButton radioButton && radioButton.IsChecked == true)
                {
                    return int.Parse(radioButton.Content.ToString());
                }
            }
            return 0; // Default value if nothing is selected
        }
        private int GetSelectedRating(StackPanel ratingGroup)
        {
            foreach (var child in ratingGroup.Children)
            {
                
                if (child is RadioButton radioButton && radioButton.IsChecked == true)
                {
                    return int.Parse(radioButton.Content.ToString());
                }
            }
            return 0; // Ako ništa nije odabrano
        }

        private void ResetControls()
        {
            // Resetiramo sve RadioButton-e
            foreach (var child in cleanlinessRatingUniformGrid.Children)
            {
                if (child is RadioButton radioButton)
                {
                    radioButton.IsChecked = false;
                }
            }

            foreach (var child in ruleFollowingRatingUniformGrid.Children)
            {
                if (child is RadioButton radioButton)
                {
                    radioButton.IsChecked = false;
                }
            }

            // Resetiramo TextBox za komentar
            commentTextBox.Text = "";
        }
    }
}
