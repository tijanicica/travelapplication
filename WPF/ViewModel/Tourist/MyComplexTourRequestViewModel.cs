using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Autofac;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Utils;
using BookingApp.WPF.View;

namespace BookingApp.WPF.ViewModel.Tourist;

public class MyComplexTourRequestViewModel
{
    private User _loggedUser;
    private IComplexTourPartService _complexTourPartService;
    private IComplexTourRequestService _complexTourRequestService;
    private IUserService _userService;
    public ObservableCollection<ComplexTourRequestModelViewModel> ComplexTours { get; set; }
    private TouristWindow _touristWindow;
  
    
    public MyComplexTourRequestViewModel(TouristWindow touristWindow)
    {
        var app = Application.Current as App;
        _loggedUser = app.LoggedUser; 
        _complexTourPartService = Injector.Container.Resolve<IComplexTourPartService>();
        _complexTourRequestService = Injector.Container.Resolve<IComplexTourRequestService>();
        _userService = Injector.Container.Resolve<IUserService>();
        _touristWindow = touristWindow;
        
        InitializeComplexTours();
    }
    
    private void InitializeComplexTours()
    {
        ComplexTours = new ObservableCollection<ComplexTourRequestModelViewModel>();

        foreach (var complexTourRequest in _complexTourRequestService.GetAll())
        {
            ComplexTourRequestModelViewModel complexTourRequestModelViewModel = new ComplexTourRequestModelViewModel();
            
            bool isInvalid = false;
            bool allPartsAccepted = true;
            
            List<ComplexTourPart> complexTourParts = new List<ComplexTourPart>();
            foreach (int complexTourPartId in complexTourRequest.tourParts)
            {
                ComplexTourPart complexTourPart = _complexTourPartService.GetById(complexTourPartId);
                
                if (complexTourPart != null)
                {
                    complexTourParts.Add(complexTourPart);

                    if (complexTourPart.Status == ComplexTourPartStatus.Pending) //status na cekannu dela ture
                    {
                        allPartsAccepted = false; //nisu svi prihvaceni

                        if (complexTourPart.BeginDate.AddDays(-2) < DateTime.Now)
                        {
                            isInvalid = true; //nevazeca
                        }
                    }
                    
                }

                ComplexTourPartModelViewModel complexTourPartModelViewModel = new ComplexTourPartModelViewModel();
                complexTourPartModelViewModel.Location = complexTourPart.Location.Country + ", " + complexTourPart.Location.City;
                complexTourPartModelViewModel.AcceptedDate = "Not assigned yet";
                complexTourPartModelViewModel.TourGuideName = "Not assigned yet";
                complexTourPartModelViewModel.StatusPart = "Pending";
               
                if (complexTourPart.Status == ComplexTourPartStatus.Accepted)
                {
                    complexTourPartModelViewModel.StatusPart = "Accepted";
                    complexTourPartModelViewModel.AcceptedDate = complexTourPart.AcceptedDate.ToString();
                    complexTourPartModelViewModel.TourGuideName = _userService.GetUsernameById(complexTourPart.TourGuideId);
                }
                complexTourRequestModelViewModel.ComplexTourParts.Add(complexTourPartModelViewModel);
            }
        
            string complexTourStatus;
            if (isInvalid)
            {
                complexTourStatus = ComplexTourRequestStatus.Invalid.ToString();
            }
            else if (allPartsAccepted)
            {
                complexTourStatus = ComplexTourRequestStatus.Accepted.ToString();
            }
            else
            {
                complexTourStatus = ComplexTourRequestStatus.Pending.ToString();
            }
            complexTourRequestModelViewModel.Status = complexTourStatus;
            ComplexTours.Add(complexTourRequestModelViewModel);
        }
    }
   
}