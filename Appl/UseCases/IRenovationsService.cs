using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BookingApp.Domain.Model;

namespace BookingApp.Appl.UseCases;

public interface IRenovationsService
{
    public void Add(Renovations renovations);
    public List<Renovations> GetFinishedRenovations(int oId);
    public ObservableCollection<Renovations> GetFutureRenovations(int oId);
    public Renovations Remove(Renovations renovation);
    public ObservableCollection<Tuple<DateTime, DateTime>> FindAvailableTimeSpans(Renovations renovation);
    public void Delete(Renovations renovation);
    public ObservableCollection<Renovations> GetRenovationsForAccommodation(int accommodationId);

    public bool IsReservationRenOverlapping(Renovations renovation, DateTime currentStartDate,
        DateTime currentEndDate);
    bool IsReservationValid(Accommodation accommodation, AccommodationReservation accommodationReservation);

    ObservableCollection<AccommodationReservation> FindAvailableReservations(Accommodation accommodation,
        AccommodationReservation reservation);

   


    /*



    public List<Renovations> GetAll();
     */
}