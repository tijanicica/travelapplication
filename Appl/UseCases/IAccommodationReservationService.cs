using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BookingApp.Domain.Model;
using BookingApp.Dto;
using BookingApp.WPF.ViewModel.Owner;

namespace BookingApp.Appl.UseCases;

public interface IAccommodationReservationService
{
  //  bool IsReservationValid(Accommodation accommodation, AccommodationReservation accommodationReservation);

 //   ObservableCollection<AccommodationReservation> FindAvailableReservations(Accommodation accommodation,
   //    AccommodationReservation reservation);

    ObservableCollection<AccommodationReservation> GetAllReservations();
    void Save(AccommodationReservation accommodationReservation);
    ObservableCollection<GuestDto> GetGuestsForReview(int ownerId);
    ObservableCollection<AccommodationReservation> GetAllReservationsByGuestId(int guestId);

    IEnumerable<AccommodationReservation> GetApprovedReservations(int guestId);
    IEnumerable<AccommodationReservation> GetRejectedReservations(int guestId);
    IEnumerable<AccommodationReservation> GetPendingReservations(int guestId);
    public IEnumerable<AccommodationReservation> GetReservationsNotInOtherListsAndNotInReschedule(int guestId);
    public void RemoveReservation(int reservationId);

    public List<DateTime> GetAvailableDates(int accommodationId, DateTime newStartDate, DateTime newEndDate,
        int duration);

    public List<AccommodationReservation> GetPreviousReservations(int guestId);
    public bool CanRateAccommodation(int reservationId);
    //public List<AccommodationReservation> GetAllReservationsByAccomodationId(int accommodationId);
    public bool CanGuestViewReview(int reservationId);
    public bool isAccommodationReservedInGivenTimeSpan(DateTime date, int length, int accId);


    public ObservableCollection<StatisticsAccommodation> StatsForAccommodation(int accommodationID);
    public ObservableCollection<StatisticsAccommodation> GetStatsPerYear(int accommodationID, string Name);
    public int GetNumberOfEditedReservationsInGivenYearForGivenAccommodationID(int year, int accomID);
    public int GetNumberOfCanceledReservationsInGivenYearForGivenAccommodationID(int year, int accomID);
    public int GetNumberOfReservationsInGivenYearForGivenAccommodationID(int year, int accomID);
    public List<AccommodationReservation> GetAllReservationsByAccomodationId(int accommodationId);
    public int GetBussiestYear(ObservableCollection<StatisticsAccommodation> statsByYear, Accommodation accommodation);
    public int GetMaxYear(ObservableCollection<StatisticsAccommodation> statsByYear);

     AccommodationReservation GetById(int reservationId);


    AccommodationReservation Update(AccommodationReservation accommodationReservation);

    public bool checkDatesOverlap(DateTime renovationBegining, DateTime renovationEnding,
        DateTime reservationBegining, DateTime reservationEnding);

    public int GetAccommodationOcupiedDays(int month, int accommID);

    public int GetNumberOfCanceledReservationsInGivenMonthForGivenAccommodationID(int month, int year, int accomID);


    public int GetNumberOfReservationsInGivenMonthForGivenAccommodationID(int month, int year, int accommID);

    public int GetNumberOfEditedReservationsInGivenMonthForGivenAccommodationID(int month, int year, int accommID);



    public ObservableCollection<StatisticsAccommodation>
        GetStatsPerMonth(StatisticsAccommodation statsForAccommodation);

    public int GetNumberOfRenovationSuggestionsInGivenMonthForGivenAccommodation(int month, int year, int accomID);
        public int GetBussiestMonth(ObservableCollection<StatisticsAccommodation> statsByMonth);

        public int GetMaxMonth(ObservableCollection<StatisticsAccommodation> statsByMonth);
        public List<AccommodationReservation> GetByUser(User user);



}