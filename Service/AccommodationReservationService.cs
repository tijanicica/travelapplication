using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Dto;
using BookingApp.Repository;
using BookingApp.WPF.ViewModel.Owner;

namespace BookingApp.Service
{
    public class AccommodationReservationService : IAccommodationReservationService
    {
        private readonly IAccommodationReservationRepository _accommodationReservationRepository;
        private readonly IAccommodationService _accommodationService;
        private readonly IAccommodationReviewService _accommodationReviewService;
        private readonly IUserService _userService;
        private readonly IReservationRescheduleService _reservationRescheduleService;
        private readonly IOwnerNotificationService _ownerNotificationService;
        private readonly IGuestReviewService _guestReviewService;

        public AccommodationReservationService(IAccommodationReservationRepository accommodationReservationRepository,
            IAccommodationService accommodationService, IUserService userService,
            IReservationRescheduleService reservationRescheduleService,
            IOwnerNotificationService ownerNotificationService, IGuestReviewService guestReviewService,
            IAccommodationReviewService accommodationReviewService)
        {
            _accommodationReservationRepository = accommodationReservationRepository;
            _accommodationService = accommodationService;
            _userService = userService;
            _reservationRescheduleService = reservationRescheduleService;
            _ownerNotificationService = ownerNotificationService;
            _guestReviewService = guestReviewService;
            _accommodationReviewService = accommodationReviewService;

        }

/*
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

            while (currentEndDate <= endDate)
            {
                bool isAvailable = true;
                foreach (AccommodationReservation reservation in accommodation.Reservations)
                {
                    if (IsReservationOverlapping(reservation, currentStartDate, currentEndDate))
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
*/
        public ObservableCollection<AccommodationReservation> GetAllReservations()
        {
            return _accommodationReservationRepository.GetAllReservations();
        }



        public void Save(AccommodationReservation accommodationReservation)
        {
            _accommodationReservationRepository.Save(accommodationReservation);
        }

        public ObservableCollection<GuestDto> GetGuestsForReview(int ownerId)
        {
            ObservableCollection<AccommodationReservation> accommodationReservations =
                _accommodationReservationRepository.GetAllReservations();
            ObservableCollection<GuestDto> guestsDto = new ObservableCollection<GuestDto>();
            foreach (AccommodationReservation accommodationReservation in accommodationReservations)
            {
                DateTime reviewDeadline = accommodationReservation.EndDate;


                if (reviewDeadline <= DateTime.Now &&
                    reviewDeadline.AddDays(5.0) >= DateTime.Now) //znaci da je zavrsio posetu
                {
                    Accommodation accommodation =
                        _accommodationService.GetAccommodationById(accommodationReservation.AccommodationId);

                    if (accommodation.OwnerId == ownerId)
                    {
                        guestsDto.Add(new GuestDto
                        {
                            Id = accommodationReservation.GuestId,
                            Username = _userService.GetUsernameById(accommodationReservation.GuestId),
                            AccommodationName = accommodation.Name,
                            StartDate = accommodationReservation.StartDate,
                            EndDate = accommodationReservation.EndDate
                        });
                    }
                }
            }


            return guestsDto;

        }

        public ObservableCollection<AccommodationReservation> GetAllReservationsByGuestId(int guestId)
        {

            var allReservations = _accommodationReservationRepository.GetAllReservations();

            var reservationsForGuest = new ObservableCollection<AccommodationReservation>(
                allReservations.Where(reservation => reservation.GuestId == guestId)
            );

            return reservationsForGuest;
        }


        public IEnumerable<AccommodationReservation> GetApprovedReservations(int guestId)
        {
            var acceptedReschedules = _reservationRescheduleService.GetAcceptedReschedules(guestId);

            var approvedReservationIds = acceptedReschedules.Select(reschedule => reschedule.ReservationId);
            var guestReservations = GetAllReservationsByGuestId(guestId);

            var approvedReservations =
                guestReservations.Where(reservation => approvedReservationIds.Contains(reservation.Id));

            return approvedReservations;
        }

        public IEnumerable<AccommodationReservation> GetRejectedReservations(int guestId)
        {
            var rejectedReschedules = _reservationRescheduleService.GetRejectedReschedules(guestId);

            var rejectedReservationIds = rejectedReschedules.Select(reschedule => reschedule.ReservationId);

            var guestReservations = GetAllReservationsByGuestId(guestId);

            var rejectedReservations =
                guestReservations.Where(reservation => rejectedReservationIds.Contains(reservation.Id));

            return rejectedReservations;
        }

        public IEnumerable<AccommodationReservation> GetPendingReservations(int guestId)
        {
            var pendingReschedules = _reservationRescheduleService.GetPendingReschedules(guestId);

            var pendingReservationIds = pendingReschedules.Select(reschedule => reschedule.ReservationId);

            var guestReservations = GetAllReservationsByGuestId(guestId);

            var pendingReservations =
                guestReservations.Where(reservation => pendingReservationIds.Contains(reservation.Id));

            var updatedPendingReservations = new List<AccommodationReservation>();

            foreach (var reservation in pendingReservations)
            {
                var reschedule = pendingReschedules.FirstOrDefault(r => r.ReservationId == reservation.Id);

                if (reschedule != null)
                {
                    var updatedReservation = new AccommodationReservation
                    {
                        Id = reservation.Id,
                        StartDate = reschedule.NewStartDate,
                        EndDate = reschedule.NewEndDate,
                        GuestNumber = reservation.GuestNumber,
                        Duration = (reschedule.NewEndDate - reschedule.NewStartDate).Days,
                        AccommodationId = reservation.AccommodationId,
                        GuestId = reservation.GuestId
                    };

                    updatedPendingReservations.Add(updatedReservation);
                }
                else
                {
                    updatedPendingReservations.Add(reservation);
                }
            }

            return updatedPendingReservations;
        }




        public IEnumerable<AccommodationReservation> GetReservationsNotInOtherListsAndNotInReschedule(int guestId)
        {
            var allReservations = GetAllReservationsByGuestId(guestId);
            var approvedReservations = GetApprovedReservations(guestId);
            var pendingReservations = GetPendingReservations(guestId);
            var rejectedReservations = GetRejectedReservations(guestId);
            var previousReservations = GetPreviousReservations(guestId);
            var rescheduleReservations = _reservationRescheduleService.GetAll(guestId)
                .Select(reschedule => reschedule.ReservationId);

            var reservationsNotInOtherLists = allReservations.Where(reservation =>
                !approvedReservations.Any(approved => approved.Id == reservation.Id) &&
                !pendingReservations.Any(pending => pending.Id == reservation.Id) &&
                !rejectedReservations.Any(rejected => rejected.Id == reservation.Id) &&
                !previousReservations.Any(previous => previous.Id == reservation.Id) &&
                !rescheduleReservations.Contains(reservation.Id)
            );

            return reservationsNotInOtherLists;
        }

        public void RemoveReservation(int reservationId)
        {
            var accommodations = _accommodationService.GetAllAccommodations();
            foreach (var accommodation in accommodations)
            {
                if (accommodation.Reservations != null)
                {
                    var reservationToRemove = accommodation.Reservations.FirstOrDefault(r => r.Id == reservationId);
                    if (reservationToRemove != null)
                    {
                        accommodation.Reservations.Remove(reservationToRemove);
                        _accommodationService.Delete(accommodation.Id);


                        _accommodationService.Save(accommodation);

                        break;
                    }
                }
            }

            _accommodationReservationRepository.RemoveReservation(reservationId);

        }



        public AccommodationReservation GetById(int reservationId)
        {
            return _accommodationReservationRepository.GetById(reservationId);
        }

        public AccommodationReservation Update(AccommodationReservation accommodationReservation)
        {
            return _accommodationReservationRepository.Update(accommodationReservation);

        }






        public List<DateTime> GetAvailableDates(int accommodationId, DateTime newStartDate, DateTime newEndDate,
            int duration)
        {
            if (!DatesAreValid(newStartDate, newEndDate, duration))
            {
                return new List<DateTime>();
            }

            var reservations = _accommodationReservationRepository.GetAllReservationsByAccomodationId(accommodationId);
            var overlappingReservations =
                reservations.Where(r => ReservationOverlaps(r.StartDate, r.EndDate, newStartDate, newEndDate));

            var availableDates = new List<DateTime>();
            for (DateTime date = newStartDate; date <= newEndDate; date = date.AddDays(1))
            {
                if (!overlappingReservations.Any(r => ReservationOverlaps(date, date, r.StartDate, r.EndDate)))
                {
                    availableDates.Add(date);
                }
            }

            return availableDates;
        }

        private bool DatesAreValid(DateTime newStartDate, DateTime newEndDate, int duration)
        {
            return (newEndDate - newStartDate).Days == duration;
        }

        private bool ReservationOverlaps(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            return (start1 <= end2 && end1 >= start2) || (start2 <= end1 && end2 >= start1);
        }





        public List<AccommodationReservation> GetPreviousReservations(int guestId)
        {
            return GetAllReservationsByGuestId(guestId)
                .Where(r => r.EndDate < System.DateTime.Today)
                .ToList();
        }

        public bool CanRateAccommodation(int reservationId)
        {
            AccommodationReservation reservation =
                _accommodationReservationRepository.GetReservationById(reservationId);

            if (reservation != null)
            {
                DateTime endDate = reservation.EndDate;
                DateTime currentDate = DateTime.Now;

                if ((currentDate - endDate).TotalDays > 5)
                {
                    return false;
                }
            }

            return true; // moze da oceni
        }

        public List<AccommodationReservation> GetAllReservationsByAccomodationId(int accommodationId)
        {
            return _accommodationReservationRepository.GetAllReservationsByAccomodationId(accommodationId);
        }

        public bool CanGuestViewReview(int reservationId)
        {
            AccommodationReservation reservation = _accommodationReservationRepository.GetById(reservationId);
            Accommodation accommodation = _accommodationService.GetAccommodationById(reservation.AccommodationId);


            if (reservation != null)
            {
                List<GuestReview> ownerGuestReviews = _guestReviewService.GetByOwnerId(accommodation.OwnerId);

                return ownerGuestReviews.Any(
                    gr => gr.GuestId == reservation.GuestId && gr.ReservationId < reservationId);
            }

            return false;
        }


        public bool isAccommodationReservedInGivenTimeSpan(DateTime date, int length, int accId)
        {
            DateTime reservationBeginingDateOnly;
            DateTime reservationEndDateOnly;
            foreach (AccommodationReservation r in GetAllReservationsByAccomodationId(accId))
            {
                reservationBeginingDateOnly = new DateTime(r.StartDate.Year, r.StartDate.Month, r.StartDate.Day);
                reservationEndDateOnly = new DateTime(r.EndDate.Year, r.EndDate.Month, r.EndDate.Day);
                if (checkDatesOverlap(date, date.AddDays(length), reservationBeginingDateOnly, reservationEndDateOnly))
                {
                    return true;
                }
            }

            return false;
        }

        public bool checkDatesOverlap(DateTime renovationBegining, DateTime renovationEnding,
            DateTime reservationBegining, DateTime reservationEnding)
        {
            if (renovationBegining <= reservationEnding && reservationBegining <= renovationEnding)
            {
                return true;
            }

            return false;
        }


        public ObservableCollection<StatisticsAccommodation> StatsForAccommodation(int accommodationID)
        {
            ObservableCollection<StatisticsAccommodation> statsByYearForGivenAccommodation =
                new ObservableCollection<StatisticsAccommodation>();

            statsByYearForGivenAccommodation = GetStatsPerYear(accommodationID,
                _accommodationService.GetAccommodationById(accommodationID).Name);

            return statsByYearForGivenAccommodation;
        }

        public ObservableCollection<StatisticsAccommodation> GetStatsPerYear(int accommodationID, string Name)
        {

            ObservableCollection<StatisticsAccommodation> statisticsForAccommodations =
                new ObservableCollection<StatisticsAccommodation>();

            foreach (AccommodationReservation r in _accommodationReservationRepository
                         .GetAllReservationsByAccomodationId(accommodationID))
            {
                StatisticsAccommodation stat = new StatisticsAccommodation();

                //sve radi kako treba al prvi put udje u if a drugi put kad bi isto trebalo ne udje
                if (!statisticsForAccommodations.Any(s => s.Year == r.StartDate.Year) ||
                    statisticsForAccommodations.Count() == 0)
                {
                    stat.AccommodationId = accommodationID;
                    stat.AccommodationName = Name;
                    stat.Year = r.StartDate.Year;
                    stat.ReservationsCount =
                        GetNumberOfReservationsInGivenYearForGivenAccommodationID(r.StartDate.Year,
                            accommodationID); //ovo radi dobro
                    stat.ReschedulingCount =
                        GetNumberOfEditedReservationsInGivenYearForGivenAccommodationID(r.StartDate.Year,
                            accommodationID);
                    stat.CancellationsCount =
                        GetNumberOfCanceledReservationsInGivenYearForGivenAccommodationID(r.StartDate.Year,
                            accommodationID);
                    stat.RenovationSuggestionsCount =
                        GetNumberOfRenovationSuggestionsInGivenYearForGivenAccommodation(r.StartDate.Year,
                            accommodationID);
                    statisticsForAccommodations.Add(stat);
                }
            }

            return statisticsForAccommodations;
        }

        public int GetNumberOfRenovationSuggestionsInGivenYearForGivenAccommodation(int year, int accomID)
        {

            int brojac = 0;
            foreach (var rr in _accommodationReviewService.GetAll())
            {
                if (rr.AccommodationID == accomID && rr.RenovationUrgencyLevel != 1 &&
                    rr.ReviewDate.Year == year)
                {
                    brojac++;
                }
            }

            return brojac;
        }

        public int GetNumberOfEditedReservationsInGivenYearForGivenAccommodationID(int year, int accomID)
        {
            int brojac = 0;
            foreach (var rr in _reservationRescheduleService.GetAlll())
            {
                if (rr.AccommodationId == accomID && rr.ReschedulingAnswerStatus == ReschedulingStatus.Accepted &&
                    rr.NewStartDate.Year == year)
                {
                    brojac++;
                }
            }

            return brojac;
        }

        public int GetNumberOfCanceledReservationsInGivenYearForGivenAccommodationID(int year, int accomID)
        {
            int brojac = 0;
            foreach (var rr in _reservationRescheduleService.GetAlll())
            {
                if (rr.AccommodationId == accomID && rr.ReschedulingAnswerStatus == ReschedulingStatus.Rejected &&
                    rr.NewStartDate.Year == year)
                {
                    brojac++;
                }
            }

            return brojac;
        }

        public int GetNumberOfReservationsInGivenYearForGivenAccommodationID(int year, int accomID)
        {
            int brojac = 0;
            foreach (AccommodationReservation r in _accommodationReservationRepository
                         .GetAllReservationsByAccomodationId(accomID))
            {
                if (year == r.StartDate.Year)
                {
                    brojac++;
                }
            }

            return brojac;
        }

//_accommodationReservationRepository.GetAllReservationsByAccomodationId(accommodation.Id)
        public int GetBussiestYear(ObservableCollection<StatisticsAccommodation> statsByYear,
            Accommodation accommodation)
        {
            foreach (StatisticsAccommodation s in statsByYear)
            {
                // Ispravio sam da koristi AccommodationReservationService za dobijanje rezervacija
                foreach (AccommodationReservation r in _accommodationReservationRepository
                             .GetAllReservationsByAccomodationId(accommodation.Id))
                {
                    if (r.StartDate.Year == s.Year)
                    {
                        // Proverimo da li se vrednosti pravilno dodaju
                        Console.WriteLine(
                            $"Dodato dana: {(r.EndDate.Date - r.StartDate.Date).Days} za godinu {s.Year}");
                        s.sumOfDaysAccommodationWasOcupiedInAYear += (r.EndDate.Date - r.StartDate.Date).Days;
                    }
                }
            }

            return GetMaxYear(statsByYear);
        }

        public int GetMaxYear(ObservableCollection<StatisticsAccommodation> statsByYear)
        {
            int maxDays = statsByYear.First().sumOfDaysAccommodationWasOcupiedInAYear;
            int maxYear = statsByYear.First().Year;
            foreach (var s in statsByYear)
            {
                if (s.sumOfDaysAccommodationWasOcupiedInAYear > maxDays)
                {
                    maxDays = s.sumOfDaysAccommodationWasOcupiedInAYear;
                    maxYear = s.Year;
                }
            }

            return maxYear;
        }

        public int GetAccommodationOcupiedDays(int month, int accommID)
        {
            int numberofDaysOccupied = 0;
            foreach (AccommodationReservation r in _accommodationReservationRepository
                         .GetAllReservationsByAccomodationId(accommID))
            {
                if (r.StartDate.Month == month)
                {
                    numberofDaysOccupied += (r.EndDate.Date - r.StartDate.Date).Days;
                }
            }

            return numberofDaysOccupied;
        }


        public int GetNumberOfCanceledReservationsInGivenMonthForGivenAccommodationID(int month, int year, int accomID)
        {
            int brojac = 0;
            foreach (var rr in _reservationRescheduleService.GetAlll())
            {
                if (year == rr.NewStartDate.Year && rr.AccommodationId == accomID &&
                    rr.ReschedulingAnswerStatus == ReschedulingStatus.Rejected &&
                    rr.NewStartDate.Month == month)
                {
                    brojac++;
                }
            }

            return brojac;
        }

        public int GetNumberOfReservationsInGivenMonthForGivenAccommodationID(int month, int year, int accommID)
        {
            int brojac = 0;
            foreach (AccommodationReservation r in _accommodationReservationRepository
                         .GetAllReservationsByAccomodationId(accommID))
            {
                if (year == r.StartDate.Year && month == r.StartDate.Month)
                {
                    brojac++;
                }
            }

            return brojac;
        }

        public int GetNumberOfEditedReservationsInGivenMonthForGivenAccommodationID(int month, int year, int accommID)
        {
            int brojac = 0;

            foreach (var rr in _reservationRescheduleService.GetAlll())
            {
                if (rr.AccommodationId == accommID && rr.ReschedulingAnswerStatus == ReschedulingStatus.Accepted &&
                    rr.NewStartDate.Year == year && rr.NewStartDate.Month == month)
                {
                    brojac++;
                }
            }

            return brojac;
        }


        public ObservableCollection<StatisticsAccommodation> GetStatsPerMonth(
            StatisticsAccommodation statsForAccommodation)
        {
            ObservableCollection<StatisticsAccommodation> statisticsForAccommodations =
                new ObservableCollection<StatisticsAccommodation>();

            foreach (AccommodationReservation r in _accommodationReservationRepository
                         .GetAllReservationsByAccomodationId(statsForAccommodation.AccommodationId))
            {
                StatisticsAccommodation stat = new StatisticsAccommodation();

                if (r.StartDate.Year == statsForAccommodation.Year)
                {
                    //sve radi kako treba al prvi put udje u if a drugi put kad bi isto trebalo ne udje
                    if (!statisticsForAccommodations.Any(s => s.Month == r.StartDate.Month) ||
                        statisticsForAccommodations.Count() == 0)
                    {
                        stat.AccommodationId = statsForAccommodation.AccommodationId;
                        stat.AccommodationName = statsForAccommodation.AccommodationName;
                        stat.Year = statsForAccommodation.Year;
                        stat.Month = r.StartDate.Month;
                        stat.sumOfDaysAccommodationWasOcupiedInAYear =
                            GetAccommodationOcupiedDays(r.StartDate.Month, statsForAccommodation.AccommodationId);
                        stat.ReservationsCount =
                            GetNumberOfReservationsInGivenMonthForGivenAccommodationID(r.StartDate.Month, stat.Year,
                                statsForAccommodation.AccommodationId); //ovo radi dobro
                        stat.ReschedulingCount =
                            GetNumberOfEditedReservationsInGivenMonthForGivenAccommodationID(r.StartDate.Month,
                                stat.Year, statsForAccommodation.AccommodationId);
                        stat.CancellationsCount =
                            GetNumberOfCanceledReservationsInGivenMonthForGivenAccommodationID(r.StartDate.Month,
                                stat.Year, statsForAccommodation.AccommodationId);
                        stat.RenovationSuggestionsCount =
                            GetNumberOfRenovationSuggestionsInGivenMonthForGivenAccommodation(r.StartDate.Month,
                                stat.Year, statsForAccommodation.AccommodationId);
                        statisticsForAccommodations.Add(stat);
                    }
                }
            }

            return statisticsForAccommodations;
        }

        public int GetNumberOfRenovationSuggestionsInGivenMonthForGivenAccommodation(int month, int year, int accomID)
        {

            int brojac = 0;
            foreach (var rr in _accommodationReviewService.GetAll())
            {
                if (rr.AccommodationID == accomID && rr.RenovationUrgencyLevel != 1 &&
                    rr.ReviewDate.Year == year && rr.ReviewDate.Month == month)
                {
                    brojac++;
                }
            }

            return brojac;
        }
 
        public int GetBussiestMonth(ObservableCollection<StatisticsAccommodation> statsByMonth)
        {
            foreach (StatisticsAccommodation s in statsByMonth)
            {
                // Ispravio sam da koristi AccommodationReservationService za dobijanje rezervacija
                foreach(AccommodationReservation r in _accommodationReservationRepository.GetAllReservationsByAccomodationId(s.AccommodationId))
                {
                    if(r.StartDate.Month == s.Month)
                    {
                        // Proverimo da li se vrednosti pravilno dodaju
                        Console.WriteLine($"Dodato dana: {(r.EndDate.Date - r.StartDate.Date).Days} za mesec {s.Month}");
                        s.sumOfDaysAccommodationWasOcupiedInAMonth += (r.EndDate.Date - r.StartDate.Date).Days;
                    }
                }
            }
            return GetMaxMonth(statsByMonth);
        }

        public int GetMaxMonth(ObservableCollection<StatisticsAccommodation> statsByMonth)
        {
            int maxDays = statsByMonth.First().sumOfDaysAccommodationWasOcupiedInAMonth;
            int maxYearMonth = statsByMonth.First().Month;
            foreach(var s in statsByMonth)
            {
                if(s.sumOfDaysAccommodationWasOcupiedInAMonth > maxDays)
                {
                    maxDays = s.sumOfDaysAccommodationWasOcupiedInAMonth;
                    maxYearMonth = s.Month;
                }
            }
            return maxYearMonth;
        }
        
        public List<AccommodationReservation> GetByUser(User user)
        {
            return _accommodationReservationRepository.GetByUser(user);
        }
        
        
    }
}