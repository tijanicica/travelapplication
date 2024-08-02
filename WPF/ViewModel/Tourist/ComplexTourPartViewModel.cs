using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class ComplexTourPartViewModel : ViewModelBase
    {
        
        private ObservableCollection<PersonOnTourViewModel> _peopleOnTour = new ObservableCollection<PersonOnTourViewModel>();
        public ObservableCollection<PersonOnTourViewModel> PeopleOnTour
        {
            get => _peopleOnTour;
            set
            {
                _peopleOnTour = value;
                OnPropertyChanged(nameof(PeopleOnTour));
                OnPropertyChanged(nameof(IsValid));
            }
        }

        
        private Location _location = new Location();
        private string _description;
        private DateTime _beginDate = DateTime.Today.AddDays(2);
        private DateTime _endDate = DateTime.Today.AddDays(2);
       
        private string _statusPart;
        public string StatusPart
        {
            get => _statusPart;
            set
            {
                _statusPart = value;
                OnPropertyChanged(nameof(StatusPart));
            }
        }

        private DateTime _acceptedDate;
        public DateTime AcceptedDate
        {
            get => _acceptedDate;
            set
            {
                if (_acceptedDate != value)
                {
                    _acceptedDate = value;
                    OnPropertyChanged(nameof(AcceptedDate));
                }
            }
        }
        
        

        public string[]? Languages { get; set; } = new[] { "English", "Serbian", "Spanish", "French" };
        public int GuidesLanguage { get; set; } = -1;
        public Language Language { get; set; }

        private int _touristCount;
        public int TouristCount
        {
            get => _touristCount;
            set
            {
                if (_touristCount != value)
                {
                    _touristCount = value;
                    GenerateTouristInputs(_touristCount);
                    OnPropertyChanged(nameof(TouristCount));
                    OnPropertyChanged(nameof(IsValid));
                    PeopleOnTour.CollectionChanged += PeopleOnTour_CollectionChanged;
                }
            }
        }
        private void GenerateTouristInputs(int count)
        {
            PeopleOnTour.Clear();
            for (int i = 0; i < count; i++)
            {
                PeopleOnTour.Add(new PersonOnTourViewModel()); // Changed from PersonOnTour to PersonOnTourViewModel
            }
        }
        

        public Location Location
        {
            get => _location;
            set
            {
                _location = value;
                OnPropertyChanged(nameof(Location));
                OnPropertyChanged(nameof(IsValid));
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(IsValid));
            }
        }

        public DateTime BeginDate
        {
            get => _beginDate;
            set
            {
                if (_beginDate != value)
                {
                    _beginDate = value;
                    OnPropertyChanged(nameof(BeginDate));
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
                OnPropertyChanged(nameof(IsValid));
            }
        }
        
        /*
        private bool ValidateFields()
        {
            // Provera da li su polja za lokaciju i opis popunjena
            if (string.IsNullOrWhiteSpace(Location.Country) || string.IsNullOrWhiteSpace(Location.City) ||
                string.IsNullOrWhiteSpace(Description))
            {
                return false;
            }

            // Provera unosa za svakog turistu
            foreach (var person in PeopleOnTour)
            {
                if (string.IsNullOrWhiteSpace(person.Name) || string.IsNullOrWhiteSpace(person.Surname) || person.Age <= 0)
                {
                    return false;
                }
            }

            // Provera ispravnosti datuma
            if (BeginDate <= DateTime.Today || EndDate <= BeginDate)
            {
                return false;
            }

            return true;
        }
*/
       
        private void PeopleOnTour_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (PersonOnTourViewModel item in e.OldItems)
                {
                    item.PropertyChanged -= PersonOnTour_PropertyChanged;
                }
            }
        
            if (e.NewItems != null)
            {
                foreach (PersonOnTourViewModel item in e.NewItems)
                {
                    item.PropertyChanged += PersonOnTour_PropertyChanged;
                }
            }
        }
        
        private void PersonOnTour_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(IsValid));
        }
        /*
        
        public bool IsValid
        {
            get { return ValidateFields(); }
        }
        */
        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(Location.Country) || string.IsNullOrWhiteSpace(Location.City) ||
                string.IsNullOrWhiteSpace(Description) || GuidesLanguage == -1 || 
                PeopleOnTour.Any(p => string.IsNullOrWhiteSpace(p.Name) || 
                                      string.IsNullOrWhiteSpace(p.Surname) ||
                                      p.Age <= 0))
            {
                return false;
            }

            if (TouristCount <= 0 || BeginDate <= DateTime.Today || EndDate <= BeginDate)
            {
                return false;
            }

            return true;
        }
    
        public bool IsValid
        {
            get { return ValidateFields(); }
        }

    }
}
