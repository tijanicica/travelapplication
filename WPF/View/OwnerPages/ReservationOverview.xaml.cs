using System.Windows.Controls;

namespace BookingApp.WPF.View.OwnerPages;

public partial class ReservationOverview : Page
{
   /* private IReservationRescheduleService _reservationRescheduleService;
    
   // private ReservationRescheduleController _reservationRescheduleController;
    private OwnerWindow _ownerWindow ;
    private User _loggedUser;
    private List<ReservationRescheduleDto> _reservationRescheduleDtoList;
    
    private  IAccommodationService _accommodationService;
    private IAccommodationReservationService _accommodationReservationService;
    
  
    
    public ReservationOverview()
    {
        InitializeComponent();
        this.DataContext = this;
        var app = Application.Current as App;
        _reservationRescheduleService = Injector.Container.Resolve<IReservationRescheduleService>();
      //  _reservationRescheduleController = app.ReservationReschedulerController;
        //_accommodationReservationController= app.AccommodationReservationController;
        _accommodationReservationService = Injector.Container.Resolve<IAccommodationReservationService>();
        
        _loggedUser = app.LoggedUser;
        _reservationRescheduleDtoList = _reservationRescheduleService.GetAllByOwnerId(_loggedUser.Id).ToList();
    }
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public List<ReservationRescheduleDto> AllSchedulesDto
    {
        get { return _reservationRescheduleDtoList; }
        set
        {
            if (value != _reservationRescheduleDtoList)
            {
                _reservationRescheduleDtoList = value;
                OnPropertyChanged("AllSchedulesDto");
            }
        }
    }
    private void ApproveButton_Click(object sender, RoutedEventArgs e)
    {
        
        Button approveButton = sender as Button;
        ReservationRescheduleDto selectedReservationRescheduleDto = approveButton.DataContext as ReservationRescheduleDto;

        if (selectedReservationRescheduleDto != null)
        {
            ReservationReschedule reservationReschedule = _reservationRescheduleService.GetById(selectedReservationRescheduleDto.Id);
            reservationReschedule.ReschedulingAnswerStatus = ReschedulingStatus.Accepted;
            AccommodationReservation accommodationReservation = _accommodationReservationService.GetById(selectedReservationRescheduleDto.ReservationId);
            accommodationReservation.StartDate = selectedReservationRescheduleDto.NewStartDate;
            accommodationReservation.EndDate = selectedReservationRescheduleDto.NewEndDate;
            _accommodationReservationService.Update(accommodationReservation);

            _reservationRescheduleService.Update(reservationReschedule);

        }
        
    }

    private void RejectButton_Click(object sender, RoutedEventArgs e)
    {
        Button rejectButton = sender as Button;
        ReservationRescheduleDto selectedReservationRescheduleDto = rejectButton.DataContext as ReservationRescheduleDto;

        if (selectedReservationRescheduleDto != null)
        {
            // Prikazivanje MessageBox-a za unos komentara
            string comment = Microsoft.VisualBasic.Interaction.InputBox("Unesite komentar:", "Komentar", "");

            if (!string.IsNullOrEmpty(comment))
            {
                // Ažuriranje rezervacije odbijanjem i dodavanje komentara
                ReservationReschedule reservationReschedule = _reservationRescheduleService.GetById(selectedReservationRescheduleDto.Id);
                reservationReschedule.ReschedulingAnswerStatus = ReschedulingStatus.Rejected;
                reservationReschedule.RejectionComment = comment;
                _reservationRescheduleService.Update(reservationReschedule);
            }
        }
    }
*/
   public ReservationOverview()
   {
       InitializeComponent();
   }
}