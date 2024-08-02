using System.Collections.Generic;
using System.Linq;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Dto;
using BookingApp.Repository;

namespace BookingApp.Service;

public class TourReservationService : ITourReservationService
{
    private readonly ITourReservationRepository _tourReservationRepository;
    /*private readonly ITourExecutionRepository _tourExecutionRepository;
    private readonly ITourRepository _tourRepository;
    private readonly INotificationRepository _notificationRepository;*/

   // private readonly ITourReservationService _tourReservationService;
    private readonly ITourExecutionService _tourExecutionService;
    //private readonly ITourService _tourService;
    private readonly INotificationService _notificationService;
    
    
    public TourReservationService(ITourReservationRepository tourReservationRepository, ITourExecutionService tourExecutionService, INotificationService notificationService)
    {
        _tourReservationRepository = tourReservationRepository;
        _tourExecutionService = tourExecutionService;
       // _tourService = tourService;
        _notificationService = notificationService;
    }
    
    public TourReservation CreateReservation(TourReservation tourReservation)
    {
        return _tourReservationRepository.Save(tourReservation);
    }
    
    public IEnumerable<TourReservation> GetByTourExecutionId(int id)
    {
        return _tourReservationRepository.GetAll().Where(reservation => reservation.TourExecutionId == id);
    }

    public TourReservation Update(TourReservation tourReservation)
    {
       return _tourReservationRepository.Update(tourReservation);
    }
    
    public IEnumerable<PersonOnTour> GetPeopleOnTour(int touristId)
    {
        TourExecution tourExecution = _tourExecutionService.GetAll().Where(e => e.Status == Status.Started).FirstOrDefault();
        if (tourExecution != null)
        {
            TourReservation tourReservation = _tourReservationRepository.GetAll().Where(e => e.TouristId == touristId && e.TourExecutionId == tourExecution.Id).FirstOrDefault();
            if (tourReservation != null)
            {
                return tourReservation.OtherPeopleOnTour ?? Enumerable.Empty<PersonOnTour>();
            }
        }
        return Enumerable.Empty<PersonOnTour>();
    }
    
    /*public Tour GetActiveTour(int touristId)
    {
        TourExecution tourExecution = _tourExecutionService.GetAll().Where(e => e.Status == Status.Started).FirstOrDefault();
        if (tourExecution != null)
        {
            TourReservation tourReservation = _tourReservationRepository.GetAll().Where(e => e.TouristId == touristId && e.TourExecutionId == tourExecution.Id).FirstOrDefault();
            if (tourReservation != null)
            {
                Tour tour = _tourService.GetById(tourExecution.TourId);
                if (tour != null)
                {
                    return tour;
                }
            }
        }
        return null;
    }*/
    
    /*public string GetCurrentTourSpot(int touristId)
    {
        TourExecution tourExecution = _tourExecutionService.GetAll().Where(e => e.Status == Status.Started).FirstOrDefault();
        Tour tour = new Tour();
        if (tourExecution != null)
        {
            TourReservation tourReservation = _tourReservationRepository.GetAll().Where(e => e.TouristId == touristId && e.TourExecutionId == tourExecution.Id).FirstOrDefault();
            if (tourReservation != null)
            {
                tour = _tourService.GetById(tourExecution.TourId);
                TourSpot currentTourSpot = tour.TourSpots.Where(e => e.Id == tourExecution.CurrentTourSpotId).FirstOrDefault();
                if (currentTourSpot != null)
                {
                    return currentTourSpot.Description;
                }
            }
        }
        return "";
    }*/
    
    public string TouristIsArrived(int touristId)
    {
        TourExecution tourExecution = _tourExecutionService.GetAll().Where(e => e.Status == Status.Started).FirstOrDefault();
        Tour tour = new Tour();
        if (tourExecution != null)
        {
            TourReservation tourReservation = _tourReservationRepository.GetAll().Where(e => e.TouristId == touristId && e.TourExecutionId == tourExecution.Id).FirstOrDefault();
            if (tourReservation != null)
            {

                if(tourExecution.Tourists.Where(e => e.TouristId == touristId).FirstOrDefault().JoinedAtTourSpot != -1)
                {
                    return "Yes";
                }
            }
        }
        return "No";
    }    
    
    public List<PersonOnTour> GetArrivedPeopleOnTourByTouristId(int touristId, int executionId)
    {
        TourReservation tourReservation = _tourReservationRepository.GetByTouristIdAndTourExecutionId(touristId, executionId);
        
            List<PersonOnTour> people = tourReservation.OtherPeopleOnTour;
            List<PersonOnTour> arrivedPeople = new List<PersonOnTour>();
            foreach (var person in people)
            {
                if (person.Arrived)
                {
                    arrivedPeople.Add(person);
                }
            }
            return arrivedPeople;
    }

        

    
    
    /*public List<TourDto> GetAllFinishedTours(int touristId)
    {
        List<TourExecution> finishedTourExecutions = _tourExecutionService.GetAll().Where(e=>e.Status == Status.Finished).ToList();
        List<Notification> acceptedNotifications = _notificationService
            .GetAll()
            .Where(n => n.TouristId == touristId && n.IsAccepted)
            .ToList();
        List<TourDto> result = new List<TourDto>();
        foreach (var tourExecution in finishedTourExecutions)
        {
            if (acceptedNotifications.Any(n => n.TourExecutionId == tourExecution.Id) &&
                tourExecution.Tourists.Any(t => t.TouristId == touristId))
            {
                TourDto tourDto = new TourDto
                {
                    Id = tourExecution.Id,
                    Name = _tourService.GetNameById(tourExecution.TourId),
                    StartDate = tourExecution.StartDate
                };
                result.Add(tourDto);
            }
        }

        return result;
    }*/

    /*public List<TourDto> GetAllByTouristId(int touristId)
    {
        return _tourReservationRepository.GetAll().Where(e=>e.TouristId == touristId).Select(e => new TourDto
        {
            Id = e.Id,
            Name = _tourService.GetNameById(_tourExecutionService.GetTourIdByExecutionId(e.TourExecutionId)),
            StartDate = _tourExecutionService.GetById(e.TourExecutionId).StartDate,
            FirstPhoto = _tourService.GetPhotosById(_tourExecutionService.GetTourIdByExecutionId(e.TourExecutionId)).FirstOrDefault()
        }).ToList();
    }*/
    
    public IEnumerable<TourReservation> GetAll()
    {
        return _tourReservationRepository.GetAll();
    }
    

}