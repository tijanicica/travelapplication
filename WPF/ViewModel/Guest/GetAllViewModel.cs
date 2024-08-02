using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Utils;
using BookingApp.WPF.View;
using BookingApp.WPF.View.GuestPages;
using BookingApp.WPF.ViewModel.GuestPages;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class GetAllViewModel : ViewModelBase
    {
        private readonly GuestWindow _guestWindow;
        private readonly IAccommodationService _accommodationService;
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly IOwnerService _ownerService;

        public ObservableCollection<Accommodation> FilteredAccommodations { get; set; }
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public Dictionary<int, List<string>> AccommodationImagePaths { get; set; }

        public ICommand DetailsCommand { get; private set; }

        public GetAllViewModel(GuestWindow guestWindow)
        {
            _guestWindow = guestWindow;
            _accommodationService = Injector.Container.Resolve<IAccommodationService>();
            _ownerService = Injector.Container.Resolve<IOwnerService>();
            _accommodationRepository = Injector.Container.Resolve<IAccommodationRepository>();

            var allAccommodations = _accommodationService.GetAllAccommodations();
            var superOwners = _ownerService.GetAll().Where(u => u.IsSuperOwner);

            Accommodations = new ObservableCollection<Accommodation>(
                allAccommodations.Where(a => superOwners.Any(o => o.Id == a.OwnerId)));

            foreach (var accommodation in allAccommodations.Where(a => !superOwners.Any(o => o.Id == a.OwnerId)))
            {
                Accommodations.Add(accommodation);
            }

            AccommodationImagePaths = new Dictionary<int, List<string>>();
            foreach (var accommodation in allAccommodations)
            {
                List<string> imagePaths = _accommodationRepository.GetImagePathsForAccommodation(accommodation.Id);
                AccommodationImagePaths.Add(accommodation.Id, imagePaths);
            }

            DetailsCommand = new RelayCommand(ExecuteDetailsCommand);
        }

        private void ExecuteDetailsCommand(object parameter)
        {
            if (parameter is Accommodation accommodation)
            {
                DetailsViewModel detailsViewModel = new DetailsViewModel(accommodation, _guestWindow);
                DetailsPage detailsPage = new DetailsPage();
                _guestWindow.MainFrameGuestWindow.Content = detailsPage;
                detailsPage.DataContext = detailsViewModel;
                Console.WriteLine(parameter.GetType());
            }
        }


    }
}
