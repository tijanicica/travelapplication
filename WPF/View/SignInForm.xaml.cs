using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using BookingApp.Domain.Model;
using BookingApp.Repository;

namespace BookingApp.WPF.View
{
    /// <summary>
    /// Interaction logic for SignInForm.xaml
    /// </summary>
    public partial class SignInForm : Window
    {

        private readonly UserRepository _repository;
        private readonly OwnerRepository _ownerRepository;
        private readonly GuestRepository _guestRepository;
        private readonly TourGuideRepository _tourGuideRepository;
        private readonly TouristRepository _touristRepository;

        private string _username;
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
        

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SignInForm()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new UserRepository();
            _ownerRepository = new OwnerRepository();
            _guestRepository = new GuestRepository();
            _tourGuideRepository = new TourGuideRepository();
            _touristRepository = new TouristRepository();
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            User user = _repository.GetByUsername(Username);
            if (user != null)
            {
                if(user.Password == txtPassword.Password)
                {
                   
                   var app = Application.Current as App;
                   app.LoggedUser = user;
                   switch (user.Type) 
                   {
                       case(UserType.Owner):
                           
                           OwnerWindow ownerWindow = new OwnerWindow();
                           ownerWindow.Show();
                           Close();

                           break;
                       case(UserType.Guest):
                           GuestWindow guestWindow = new GuestWindow();
                           guestWindow.Show();
                           Close();

                           break;
                       case(UserType.TourGuide):
                           TourGuideWindow tourGuideWindow = new TourGuideWindow();
                           tourGuideWindow.Show();
                           Close();

                           break;
                       case(UserType.Tourist):
                           TouristWindow touristWindow = new TouristWindow();
                           touristWindow.Show();
                           Close();
                           break;
                   }
                } 
                else
                {
                    MessageBox.Show("Wrong password!");
                }
            }
            else 
            {
                MessageBox.Show("Wrong username!");
            }
            
        }
    }
}
