using System.Collections.ObjectModel;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;
using BookingApp.WPF.View.TourGuidePages;

namespace BookingApp.WPF.ViewModel.TourGuide;

public class ComplexTourPartsPageViewModel : ViewModelBase
{
    private TourGuideWindow _tourGuideWindow;
    private ComplexTourRequest _currentRequest;
    private ObservableCollection<ComplexTourPartsModelViewModel> _tourParts;
    private IComplexTourPartService _tourPartService;
    
    

    public ObservableCollection<ComplexTourPartsModelViewModel> TourParts
    {
        get => _tourParts;
        set
        {
            if (value != _tourParts)
            {
                _tourParts = value;
                OnPropertyChanged();
            }
        }
    }
    
    public ComplexTourPartsPageViewModel(TourGuideWindow tourGuideWindow, ComplexTourRequest currentRequest)
    {
        _tourGuideWindow = tourGuideWindow;
        _currentRequest = currentRequest;
        _tourPartService =  Injector.Container.Resolve<IComplexTourPartService>();
        InitializeTourParts();
        OpenOneTourPartPageCommand = new DelegateCommand(OpenOneTourPartPage);
    }

    private void InitializeTourParts()
    {
        TourParts = new ObservableCollection<ComplexTourPartsModelViewModel>();
        foreach (var tourPartId in _currentRequest.tourParts)
        {
            ComplexTourPart tourPart = _tourPartService.GetById(tourPartId);
            TourParts.Add(new ComplexTourPartsModelViewModel(tourPartId, tourPart.Language.ToString(), tourPart.Location.Country.ToString(), tourPart.BeginDate.ToShortDateString(), 
                tourPart.EndDate.ToShortDateString(), tourPart.Status.ToString()));
        }
    }
    
    public ICommand OpenOneTourPartPageCommand { get; private set; }
    
    
    private void OpenOneTourPartPage(object param)
    {
        ObservableCollection<ComplexTourPart> allTourParts = new ObservableCollection<ComplexTourPart>();
        foreach (var id in _currentRequest.tourParts)
        {
            ComplexTourPart tourPart = _tourPartService.GetById(id);
            allTourParts.Add(tourPart);
        }
        

        int tourPartId = (int)param;
        if (tourPartId != null)
        {
            ComplexTourPart currentPart = _tourPartService.GetById(tourPartId);
            _tourGuideWindow.MainFrameTourGuideWindow.NavigationService.Navigate(new OneTourPartPage
            {
                DataContext = new OneTourPartPageViewModel(_tourGuideWindow,
                    currentPart, allTourParts)
            });
        }
    }
}