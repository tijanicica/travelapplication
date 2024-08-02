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

public class AllRenovationsPageViewModel: ViewModelBase, INotifyPropertyChanged
{
    private User _loggedUser;
    private OwnerWindow _ownerWindow;
    private IRenovationsService _renovationsService;
    public ObservableCollection<Renovations> FinishedRenovations { get; set; }
    public ObservableCollection<Renovations> FutureRenovations { get; set; }


    public AllRenovationsPageViewModel(OwnerWindow ownerWindow)
    {
        var app = Application.Current as App;
        _ownerWindow = ownerWindow;
        _loggedUser = app.LoggedUser; 
        _renovationsService = Injector.Container.Resolve<IRenovationsService>();
       // FinishedRenovations = _renovationsService.GetFinishedRenovations();
       // FutureRenovations = _renovationsService.GetFutureRenovations();
     
       InitializeFinished();
       InitializeFuture();
        
      
        CancelCommand = new DelegateCommand(CancelAction);

    }
    public ICommand CancelCommand { get; private set; }
    private void CancelAction(object parameter)
    {
        var renovation = parameter as Renovations;
        if ((renovation.BeginingDate - DateTime.Today).TotalDays > 5)
        {
            _renovationsService.Delete(renovation);
            FutureRenovations.Remove(renovation);
            MessageBox.Show($"Renovation is canceled!");
        }
        else
        {
            MessageBox.Show("Renovation cannot be canceled. Less than 5 days remaining.");
        }
       
    }
    /*
   public ObservableCollection<Renovations> FutureRenovations
   {
       get { return _futureRenovations; }
       set
       {
           _futureRenovations = value;
           OnPropertyChanged(nameof(FutureRenovations));
       }
   }

   public List<Renovations> FinishedRenovations
   {
       get { return finishedRenovations; }
       set
       {
           finishedRenovations = value;
           OnPropertyChanged(nameof(FinishedRenovations));
       }
   }
   */
    private void InitializeFinished()
    {
        // FinishedRenovations = _renovationsService.GetFinishedRenovations();
        // FutureRenovations = _renovationsService.GetFutureRenovations();
        FinishedRenovations = new ObservableCollection<Renovations>();
        
        foreach (var tour in _renovationsService.GetFinishedRenovations(_loggedUser.Id))
        {
      
            FinishedRenovations.Add(new Renovations(
                    tour.Id,
                    tour.AccommodationToRenovateId,
                    tour.AccommodationToRenovateName,
                    tour.WantedBeginingDate,
                    tour.WantedEndingDate,
                    tour.BeginingDate,
                    tour.EndingDate,
                    tour.Description,
                    tour.Length
                )
            );
        }
    }
    private void InitializeFuture()
    {
      
        FutureRenovations = new ObservableCollection<Renovations>();
        
        foreach (var tour in _renovationsService.GetFutureRenovations(_loggedUser.Id))
        {
      
            FutureRenovations.Add(new Renovations(
                    tour.Id,
                    tour.AccommodationToRenovateId,
                    tour.AccommodationToRenovateName,
                    tour.WantedBeginingDate,
                    tour.WantedEndingDate,
                    tour.BeginingDate,
                    tour.EndingDate,
                    tour.Description,
                    tour.Length
                )
            );
        }
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
}