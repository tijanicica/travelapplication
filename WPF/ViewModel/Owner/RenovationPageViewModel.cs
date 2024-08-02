using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

public class RenovationPageViewModel : ViewModelBase, INotifyPropertyChanged
{
    private OwnerWindow _ownerWindow;
    private User _loggedUser;
    private IRenovationsService _renovationsService;
    private Accommodation _currentAccommodation;
    public Renovations renovation { get; set; }

    public RenovationPageViewModel(OwnerWindow ownerWindow, Accommodation currentAccommodation)
    {
        _ownerWindow = ownerWindow;
        var app = Application.Current as App;
        _loggedUser = app.LoggedUser; 
        _currentAccommodation = currentAccommodation;
        _renovationsService = Injector.Container.Resolve<IRenovationsService>();
        renovation = new Renovations(); 
        
      //  renovation.WantedBeginingDate = DateTime.Now;
        //renovation.WantedEndingDate = DateTime.Now;
        CheckAvailableCommand = new DelegateCommand(CheckAvailableAction);
        SubmitCommand = new ExecuteCommand<object>(SubmitAction);
        CancelCommand = new DelegateCommand(CancelAction);
     //   SelectDatePairCommand = new DelegateCommand<DatePair>(SelectDatePairAction);
        
        
    }
    public ICommand SelectDatePairCommand { get; private set; }
    public ICommand CheckAvailableCommand { get; private set; }
    public ICommand SubmitCommand { get; private set; }
    public ICommand CancelCommand { get; private set; }
    private void SelectDatePairAction(DatePair selectedDatePair)
    {
        if (selectedDatePair != null)
        {
            BeginingDate = selectedDatePair.StartDate;
            EndingDate = selectedDatePair.EndDate;
        }
    }
    private void CancelAction(object param)
    {
     
        _ownerWindow.MainFrameOwnerWindow.NavigationService.Navigate(new MyAccommodations()
        {
            DataContext = new MyAccommodationPageViewModel(_ownerWindow)
        });
      
    }
    private void CheckAvailableAction(object param)
    {
        renovation.AccommodationToRenovateId = _currentAccommodation.Id;
        renovation.AccommodationToRenovateName = _currentAccommodation.Name;
        renovation.WantedBeginingDate = WantedBeginingDate;
        renovation.WantedEndingDate = WantedEndingDate;
        renovation.Length = Length;
        //imam listu parova datuma, sad samo treba da namestim kako ce ItemControl u xamlu da prikazuje
        DatePairs = _renovationsService.FindAvailableTimeSpans(renovation);
    
      
    }
    private void SubmitAction(object parameter)
    {
        var datesPair = parameter as Tuple<DateTime, DateTime>;
//if (_currentAccommodation.Id != null)
      // {
            Renovations renovation = new Renovations();
            renovation.AccommodationToRenovateId = _currentAccommodation.Id;
            renovation.AccommodationToRenovateName = _currentAccommodation.Name;
            renovation.BeginingDate = new DateTime(datesPair.Item1.Year, datesPair.Item1.Month, datesPair.Item1.Day);
            renovation.EndingDate = new DateTime(datesPair.Item2.Year, datesPair.Item2.Month, datesPair.Item2.Day);
            renovation.WantedBeginingDate = WantedBeginingDate;
            renovation.WantedEndingDate = WantedEndingDate ;
            //renovation.BeginingDate = BeginingDate;
            //renovation.EndingDate = EndingDate;
            renovation.Description =Description;
            renovation.Length = Length;
            
            _renovationsService.Add(renovation);
           
            MessageBox.Show($"Renovation for accommodation: {renovation.AccommodationToRenovateName} has been scheduled", "Renovation Scheduled", MessageBoxButton.OK, MessageBoxImage.Information);
        
        //}
    }
    private ObservableCollection<Tuple<DateTime, DateTime>> _datePairs;
    public ObservableCollection<Tuple<DateTime, DateTime>> DatePairs
    {
        get { return _datePairs; }
        set { _datePairs = value; OnPropertyChanged(nameof(DatePairs)); }
    }
 
        private DateTime _wantedBeginingDate;
        public DateTime WantedBeginingDate
        {
            get => _wantedBeginingDate;
            set
            {
                if (value != _wantedBeginingDate)
                {
                    _wantedBeginingDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _wantedEndingDate;
        public DateTime WantedEndingDate
        {
            get => _wantedEndingDate;
            set
            {
                if (value != _wantedEndingDate)
                {
                    _wantedEndingDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _beginingDate;
        public DateTime BeginingDate
        {
            get => _beginingDate;
            set
            {
                if (value != _beginingDate)
                {
                    _beginingDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _endingDate;
        public DateTime EndingDate
        {
            get => _endingDate;
            set
            {
                if (value != _endingDate)
                {
                    _endingDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _length;
        public int Length
        {
            get => _length;
            set
            {
                if (value != _length)
                {
                    _length = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
}

