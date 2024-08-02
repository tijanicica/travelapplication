using System.Collections.ObjectModel;
using BookingApp.Domain.Model;

// Pretpostavka da imate ViewModel za ovu stranicu

namespace BookingApp.WPF.ViewModel;

public class ReservationOverviewViewModel :  ViewModelBase
{
    private ObservableCollection<ReservationReschedule> _reservationRequests;
    public ObservableCollection<ReservationReschedule> ReservationRequests
    {
        get { return _reservationRequests; }
        set
        {
            _reservationRequests = value;
            OnPropertyChanged(nameof(ReservationRequests));
        }
    }
/*
    public ReservationOverviewViewModel()
    {
        // Inicijalizacija listu zahteva
        ReservationRequests = new ObservableCollection<ReservationReschedule>();

        // Dohvatite sve zahteve i dodajte ih u listu
        var allRequests = // Pozovite odgovarajući metod iz kontrolera za dohvatanje svih zahteva
        foreach (var request in allRequests)
        {
            ReservationRequests.Add(request);
        }
    }
    */
}