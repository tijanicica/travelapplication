using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Utils;

namespace BookingApp.Service;

public class RenovationsService : IRenovationsService

{
    private readonly IRenovationsRepository _renovationsRepository;
    private readonly IAccommodationService _accommodationService;
    private readonly IAccommodationReservationService _accommodationReservationService;
    public RenovationsService(IRenovationsRepository renovationsRepository, IAccommodationReservationService accommodationReservationService, IAccommodationService accommodationService)
    {
        _renovationsRepository = renovationsRepository;
        _accommodationReservationService = accommodationReservationService;
        _accommodationService = accommodationService;

    }

    public void Add(Renovations renovations)
    {
        _renovationsRepository.Save(renovations);
    }
    public List<Renovations> GetFinishedRenovations(int oId)
    {
        DateTime currentDate = DateTime.Today;
        List<Renovations> finishedRenovations = new List<Renovations>();
        foreach(Renovations r in GetAllByOwnerID(oId))
        {
            if(r.EndingDate < currentDate)
            {
                finishedRenovations.Add(r);
            }
        }
        return finishedRenovations;
    }
    public ObservableCollection<Renovations> GetFutureRenovations(int oId)
    {
        DateTime currentDate = DateTime.Today;
        ObservableCollection<Renovations> futureRenovations = new ObservableCollection<Renovations>();
        foreach(Renovations r in GetAllByOwnerID(oId))
        {
            if(currentDate < r.BeginingDate)
            {
                futureRenovations.Add(r);
            }
        }
        return futureRenovations;
    }
    
    public bool IsReservationValid(Accommodation accommodation, AccommodationReservation accommodationReservation)
    {
        if (accommodationReservation.StartDate.Date.CompareTo(accommodationReservation.EndDate.Date) > 0)
        {
            return false;
        }
        else if (accommodationReservation.StartDate.AddDays(accommodationReservation.Duration) >
                 accommodationReservation.EndDate.Date)
        {
            return false;
        }
        else if (accommodation.MinDuration > accommodationReservation.Duration)
        {
            return false;
        }
        else if (accommodationReservation.GuestNumber > accommodation.MaxGuestNumber)
        {
            return false;
        }

        DateTime currentStartDate = accommodationReservation.StartDate.Date;
        DateTime currentEndDate = accommodationReservation.EndDate.Date;

       // var renovationsService = Injector.Container.Resolve<IRenovationsService>();
        var renovations = GetRenovationsForAccommodation(accommodation.Id);

        foreach (var renovation in renovations)
        {
            if (IsReservationRenOverlapping(renovation, currentStartDate, currentEndDate))
            {
                return false;
            }
        }

        return true;
        
    }
    
   

        public ObservableCollection<AccommodationReservation> FindAvailableReservations(Accommodation accommodation,
            AccommodationReservation reservation)
        {
            int stayingDays = reservation.Duration;
            DateTime startDate = reservation.StartDate.Date;
            DateTime endDate = reservation.EndDate.Date;
            int guestNumber = reservation.GuestNumber;

            ObservableCollection<AccommodationReservation> availableReservations =
                new ObservableCollection<AccommodationReservation>();

            if (IsReservationValid(accommodation, reservation) )
            {
                FindAvailableReservationsInRange(accommodation, startDate, endDate, guestNumber, stayingDays,
                    availableReservations);
            }
            else
            {
                DateTime maxEndDate = reservation.StartDate.AddDays(30);
                FindAvailableReservationsInRange(accommodation, startDate, maxEndDate, guestNumber, stayingDays,
                    availableReservations);
            }

            return availableReservations;
        }
        

        private bool IsReservationOverlapping(AccommodationReservation existingReservation, DateTime currentStartDate,
            DateTime currentEndDate)
        {
            return (DateTime.Compare(existingReservation.EndDate.Date, currentStartDate) >= 0 &&
                    DateTime.Compare(existingReservation.StartDate.Date, currentEndDate) <= 0) ||
                   (DateTime.Compare(currentEndDate, existingReservation.StartDate.Date) >= 0 &&
                    DateTime.Compare(currentEndDate, existingReservation.EndDate.Date) <= 0);
        }

        private void FindAvailableReservationsInRange(Accommodation accommodation, DateTime startDate, DateTime endDate,
            int guestNumber, int stayingDays, ObservableCollection<AccommodationReservation> availableReservations)
        {
            DateTime currentStartDate = startDate;
            DateTime currentEndDate = startDate.AddDays(stayingDays);

            bool isAnyAvailable = false;

            var renovations = GetRenovationsForAccommodation(accommodation.Id);

            while (currentEndDate <= endDate)
            {
                bool isAvailable = true;

                // Provera rezervacija
                foreach (AccommodationReservation reservation in accommodation.Reservations)
                {
                    if (IsReservationOverlapping(reservation, currentStartDate, currentEndDate))
                    {
                        isAvailable = false;
                        break;
                    }
                }

                // Provera renovacija
                foreach (var renovation in renovations)
                {
                    if (IsReservationRenOverlapping(renovation, currentStartDate, currentEndDate))
                    {
                        isAvailable = false;
                        break;
                    }
                }

                if (isAvailable)
                {
                    availableReservations.Add(new AccommodationReservation(currentStartDate, currentEndDate,
                        guestNumber, stayingDays, accommodation.Id));
                    isAnyAvailable = true;
                }

                currentStartDate = currentStartDate.AddDays(1);
                currentEndDate = currentStartDate.AddDays(stayingDays);

                if (currentEndDate > endDate)
                {
                    if (isAnyAvailable)
                    {
                        break;
                    }
                    else
                    {
                        endDate = startDate.AddDays(30);
                        currentStartDate = startDate;
                        currentEndDate = currentStartDate.AddDays(stayingDays);
                    }
                }
            }
        }

    
    
    public bool IsReservationRenOverlapping(Renovations renovation, DateTime currentStartDate, DateTime currentEndDate)
    {
        return (DateTime.Compare(renovation.EndingDate.Date, currentStartDate) >= 0 &&
                DateTime.Compare(renovation.BeginingDate.Date, currentEndDate) <= 0) ||
               (DateTime.Compare(currentEndDate, renovation.BeginingDate.Date) >= 0 &&
                DateTime.Compare(currentEndDate, renovation.EndingDate.Date) <= 0);
    }
    
    
    
    
    
    
    
    public Renovations Remove(Renovations renovation)
    {
        return _renovationsRepository.removeRenovation(renovation.Id);
    }
    public ObservableCollection<Renovations> GetRenovationsForAccommodation(int accommodationId)
    {
        return new ObservableCollection<Renovations>(_renovationsRepository.GetAll().Where(r => r.AccommodationToRenovateId == accommodationId));
    }
    
    public ObservableCollection<Tuple<DateTime, DateTime>> FindAvailableTimeSpans(Renovations renovation)
    {
        ObservableCollection<Tuple<DateTime, DateTime>> datePairs = new ObservableCollection<Tuple<DateTime, DateTime>>();
        for (DateTime begining = renovation.WantedBeginingDate; begining <= renovation.WantedEndingDate; begining = begining.AddDays(1))
        {
            if(!_accommodationReservationService.isAccommodationReservedInGivenTimeSpan(begining, renovation.Length, renovation.AccommodationToRenovateId))
            {
                datePairs.Add(new Tuple<DateTime, DateTime>(begining, begining.AddDays(renovation.Length)));
            
            }
        }
        return datePairs;
    }
    public List<Renovations> GetAllByOwnerID(int ownerID) 
    {
        List<Renovations> ownersRenovations = new List<Renovations> ();
        foreach(Renovations r in _renovationsRepository.GetAll())
        {
            if(IsAccommodationByGivenOwner(r.AccommodationToRenovateId, ownerID))
            {
                ownersRenovations.Add(r);
            }
        }
        return ownersRenovations;
    }

    public bool IsAccommodationByGivenOwner(int accommodationID, int ownerID)
    {
        foreach(Accommodation a in _accommodationService.GetAllAccommodations())
        {
            if(a.Id == accommodationID && a.OwnerId == ownerID)
            {
                return true;
            }
        }
        return false;
    }
    public void Delete(Renovations renovation)
    {
        _renovationsRepository.Delete(renovation);
    }

/*
    private readonly IRenovationsRepository _renovationsRepository;
    private readonly IAccommodationReservationService _accommodationReservationService;
    private readonly IAccommodationService _accommodationService;

    public RenovationsService(IRenovationsRepository renovationsRepository,
        IAccommodationService accommodationService, IAccommodationReservationService accommodationReservationService)
    {
        _renovationsRepository = renovationsRepository;
        _accommodationService = accommodationService;
        _accommodationReservationService = accommodationReservationService;
    }

    public Renovations Remove(Renovations renovation)
    {
        return _renovationsRepository.removeRenovation(renovation.renovationID);
    }
  

    public void Add(Renovations renovations)
    {
        _renovationsRepository.Add(renovations);
    }

    public List<Renovations> GetAll()
    {
        return _renovationsRepository.GetAll();
    }



*/

}