using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;
using BookingApp.WPF.View.OwnerPages;
using DelegateCommand = BookingApp.WPF.View.DelegateCommand;



namespace BookingApp.WPF.ViewModel.Owner;

    public class StatisticsYearPageViewModel : ViewModelBase
    {
        private OwnerWindow _ownerWindow;
        private Accommodation _currentAccommodation;
        public IAccommodationService _accommodationService { get; set; }


        public IAccommodationReservationService _reservationService { get; set; }

        public ObservableCollection<StatisticsAccommodation> statsYearForSelectedAccommodation { get; set; }
        public int bussiestYear { get; set; }

        public StatisticsYearPageViewModel(OwnerWindow ownerWindow, Accommodation currentAccommodation)
        {
            _ownerWindow = ownerWindow;
            var app = Application.Current as App;
            _currentAccommodation = currentAccommodation;
            _accommodationService = Injector.Container.Resolve<IAccommodationService>();
            _reservationService = Injector.Container.Resolve<IAccommodationReservationService>();
           
            //AccommodationName = currentAccommodation.Name;
            Initialize();

            DetaileStatsCommand = new ExecuteCommand<object>(DetaileStatsAction);


        }
        private void Initialize()
        {
            statsYearForSelectedAccommodation = new ObservableCollection<StatisticsAccommodation>();

            foreach (var tour in _reservationService.StatsForAccommodation(_currentAccommodation.Id))
            {

                statsYearForSelectedAccommodation.Add(new StatisticsAccommodation(
                    tour.AccommodationId,
                    tour.AccommodationName,
                    tour.ReservationsCount,
                    tour.CancellationsCount,
                    tour.ReschedulingCount, 
                    tour.RenovationSuggestionsCount,
                    tour.Year,
                    tour.Month,
                    tour.sumOfDaysAccommodationWasOcupiedInAYear
                    )
                );
            }

         
            bussiestYear =
                _reservationService.GetBussiestYear(statsYearForSelectedAccommodation, _currentAccommodation);


        }

        public ICommand DetaileStatsCommand { get; private set; }

        public void DetaileStatsAction(object param)
        {
            var stat = param as  StatisticsAccommodation;

                _ownerWindow.MainFrameOwnerWindow.Content = new StatisticsMounthPage()
                {
                    DataContext = new StatisticsMounthPageViewModel(_ownerWindow, stat)
                };
            
        }

       


    }
