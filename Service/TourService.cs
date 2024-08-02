using System;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Dto;
using BookingApp.Parameters;
using BookingApp.Repository;

namespace BookingApp.Service;

public class TourService : ITourService
{
    private readonly ITourRepository _tourRepository;
    private readonly ITourExecutionService _tourExecutionService;
    private readonly ITourReservationService _tourReservationService;
    private readonly INotificationService _notificationService;
    private readonly IUserService _userService;
    private readonly ITourGuideService _tourGuideService;


    public TourService(ITourRepository tourRepository, ITourExecutionService tourExecutionService,
        ITourReservationService tourReservationService, INotificationService notificationService,
        IUserService userService, ITourGuideService tourGuideService)
    {
        _tourRepository = tourRepository;
        _tourExecutionService = tourExecutionService;
        _tourReservationService = tourReservationService;
        _notificationService = notificationService;
        _userService = userService;
        _tourGuideService = tourGuideService;
    }

    public IEnumerable<TourDto> GetFilteredTours(FilterTours filter)
    {
        var tours = _tourExecutionService.GetAll().Where(tour => tour.Tourists.All(tourist => tourist.TouristId != 4));

        if (!string.IsNullOrWhiteSpace(filter.Country))
        {
            tours = tours.Where(e =>
                _tourRepository.GetCountryByTourId(e.TourId)
                    .IndexOf(filter.Country, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        if (!string.IsNullOrWhiteSpace(filter.City))
        {
            tours = tours.Where(e =>
                _tourRepository.GetCityByTourId(e.TourId).IndexOf(filter.City, StringComparison.OrdinalIgnoreCase) >=
                0);
        }

        if (filter.Duration > 0)
        {
            tours = tours.Where(e => _tourRepository.GetDurationByTourId(e.TourId) == filter.Duration);
        }

        if (filter.PeopleNumber > 0)
        {
            tours = tours.Where(e => _tourRepository.GetMaxTouristNumberByTourId(e.TourId) >= filter.PeopleNumber);
        }

        if (!string.IsNullOrWhiteSpace(filter.Language) &&
            Enum.TryParse<Language>(filter.Language, true, out var parsedLanguage))
        {
            tours = tours.Where(e => _tourRepository.GetLanguageByTourId(e.TourId) == parsedLanguage);
        }


        tours = tours.Where(e => e.StartDate > DateTime.Now).Where(tour => tour.Tourists.All(tourist => tourist.TouristId != 4));

        var superGuideIds = _tourGuideService.GetAll().Where(g => g.IsSuperGuide).Select(g => g.Id).ToHashSet();
        
        var tourDtos = tours.Select(e => new TourDto
        {
            Id = e.Id,
            Name = _tourRepository.GetNameById(e.TourId) + (superGuideIds.Contains(_tourRepository.GetTourGuideIdByTourId(e.TourId)) ? "*" : ""),
            StartDate = e.StartDate,
            FirstPhoto = _tourRepository.GetPhotosById(e.TourId).FirstOrDefault()
        }).ToList();

        return tourDtos.OrderByDescending(dto => dto.Name.EndsWith("*")).ThenBy(dto => dto.Name);
        
    }
    
    public IEnumerable<TourDto> GetTourDtos()
    {
        List<TourGuide> tourGuides = _tourGuideService.GetAll();
        var superGuideIds = tourGuides.Where(g => g.IsSuperGuide).Select(g => g.Id).ToHashSet();
    
        var tourDtos = _tourExecutionService.GetAll().Where(tour => tour.Tourists.All(tourist => tourist.TouristId != 4)).Select(e =>
        {
            var tourName = _tourRepository.GetNameById(e.TourId);
            var tourGuideId = _tourRepository.GetTourGuideIdByTourId(e.TourId);
        
            if (superGuideIds.Contains(tourGuideId))
            {
                tourName += "*";
            }
        
            // Log or debug output
           // Console.WriteLine($"Tour ID: {e.Id}, Name: {tourName}, Tour Guide ID: {tourGuideId}");
        
            return new TourDto
            {
                Id = e.Id,
                Name = tourName,
                StartDate = e.StartDate,
                FirstPhoto = _tourRepository.GetPhotosById(e.TourId).FirstOrDefault()
            };
        }).ToList();
    
        return tourDtos.OrderByDescending(dto => dto.Name.EndsWith("*")).ThenBy(dto => dto.Name);
    }

    


    public IEnumerable<TourDto> GetToursTodayByTourGuideId(int tourGuideId)
    {
        return _tourExecutionService.GetAll().Where(e => e.StartDate.Date == DateTime.Today &&
                                                         _tourRepository.GetTourGuideIdByTourId(e.TourId) ==
                                                         tourGuideId)
            .Select(e => new TourDto
                { Id = e.Id, Name = _tourRepository.GetNameById(e.TourId), StartDate = e.StartDate });
    }

    public int GetTourIdByExecutionId(int executionId)
    {
        return _tourExecutionService.GetTourIdByExecutionId(executionId);
    }


    public Tour GetById(int id)
    {
        return _tourRepository.GetById(id);
    }

    public Tour Update(Tour tour)
    {
        return _tourRepository.Update(tour);
    }

    public Tour Save(Tour tour)
    {
        return _tourRepository.Save(tour);
    }


    public IEnumerable<TourDto> GetAll()
    {
        return _tourRepository.GetAll().Select(e => new TourDto
        {
            Id = e.Id,
            Name = e.Name,
        });
    }

    public IEnumerable<TourDto> GetByCountry(string country)
    {
        return _tourExecutionService.GetAll().Where(e => _tourRepository.GetCountryByTourId(e.TourId) == country)
            .Select(e => new TourDto
            {
                Id = e.Id,
                Name = _tourRepository.GetNameById(e.TourId),
                StartDate = e.StartDate,
                FirstPhoto = _tourRepository.GetPhotosById(e.TourId).FirstOrDefault()
            });
    }

    public IEnumerable<TourDto> GetByCity(string city)
    {
        return _tourExecutionService.GetAll().Where(e => _tourRepository.GetCityByTourId(e.TourId) == city).Select(
            e => new TourDto
            {
                Id = e.Id,
                Name = _tourRepository.GetNameById(e.TourId),
                StartDate = e.StartDate,
                FirstPhoto = _tourRepository.GetPhotosById(e.TourId).FirstOrDefault()
            });
    }

    public IEnumerable<TourDto> GetByDuration(double duration)
    {
        return _tourExecutionService.GetAll().Where(e => _tourRepository.GetDurationByTourId(e.TourId) == duration)
            .Select(e => new TourDto
            {
                Id = e.Id,
                Name = _tourRepository.GetNameById(e.TourId),
                StartDate = e.StartDate,
                FirstPhoto = _tourRepository.GetPhotosById(e.TourId).FirstOrDefault()
            });
    }

    public IEnumerable<TourDto> GetByMaxTouristNumber(int touristNumber)
    {
        return _tourExecutionService.GetAll()
            .Where(e => _tourRepository.GetMaxTouristNumberByTourId(e.TourId) > touristNumber).Select(e => new TourDto
            {
                Id = e.Id,
                Name = _tourRepository.GetNameById(e.TourId),
                StartDate = e.StartDate,
                FirstPhoto = _tourRepository.GetPhotosById(e.TourId).FirstOrDefault()
            });
    }

    public IEnumerable<TourDto> GetByLanguage(Language language)
    {
        return _tourExecutionService.GetAll().Where(e => _tourRepository.GetLanguageByTourId(e.TourId) == language)
            .Select(e => new TourDto
            {
                Id = e.Id,
                Name = _tourRepository.GetNameById(e.TourId),
                StartDate = e.StartDate,
                FirstPhoto = _tourRepository.GetPhotosById(e.TourId).FirstOrDefault()
            });
    }

    public bool IsBelowMaxCapacity(int touristsNumbers, int tourExecutionId)
    {
        int currentFreeSpots = GetCurrentFreeSpots(tourExecutionId);
        currentFreeSpots -= (touristsNumbers + 1);
        if (currentFreeSpots >= 0)
        {
            //ima slobodnih mesta
            return true;
        }

        return false;
    }

    public bool IsFull(int tourExecutionId)
    {
        TourExecution tourExecution = _tourExecutionService.GetById(tourExecutionId);
        if (tourExecution != null)
        {
            Tour tour = _tourRepository.GetById(tourExecution.TourId);
            List<TourReservation> tourReservations = _tourReservationService.GetAll()
                .Where(e => e.TourExecutionId == tourExecutionId).ToList();
            int totalTouristCount = 0;
            totalTouristCount = tourReservations.Sum(r => r.OtherPeopleOnTour.Count);
            int totalDeduction = 0;
            totalDeduction = tourReservations.Count + totalTouristCount;
            int result = 0;
            result = tour.MaxTouristNumber - totalDeduction;
            //ako nema slobodnog mesta vrati false
            if (result <= 1)
            {
                return false;
            }

            return true;
        }

        return false;
    }

    public int GetCurrentFreeSpots(int tourExecutionId)
    {
        TourExecution tourExecution = _tourExecutionService.GetById(tourExecutionId);

        if (tourExecution != null)
        {
            Tour tour = _tourRepository.GetById(tourExecution.TourId);
            List<TourReservation> tourReservations = _tourReservationService.GetAll()
                .Where(e => e.TourExecutionId == tourExecutionId).ToList();

            int totalTouristCount = tourReservations.Sum(r => r.OtherPeopleOnTour.Count);
            int totalDeduction = tourReservations.Count + totalTouristCount;
            return tour.MaxTouristNumber -= totalDeduction;
        }

        return -1;
    }

    public IEnumerable<Tour> GetByTourGuideId(int tourGuideId)
    {
        return _tourRepository.GetAll().Where(tour => tour.TourGuideId == tourGuideId);
    }

    public IEnumerable<TourSpot> GetTourSpotsByTourId(int tourId)
    {
        var Tour = _tourRepository.GetById(tourId);
        var firstTourSpot = Tour.TourSpots.FirstOrDefault();
        if (firstTourSpot != null)
        {
            firstTourSpot.Visited = true;
            _tourRepository.Update(Tour);
        }

        return Tour.TourSpots;
    }

    public IEnumerable<TourDto> GetTourDtosByLocation(int tourExecutionId)
    {
        TourExecution tourExecution = _tourExecutionService.GetById(tourExecutionId);
        string country = _tourRepository.GetCountryByTourId(tourExecution.TourId);
        string city = _tourRepository.GetCityByTourId(tourExecution.TourId);

        return _tourExecutionService.GetAll()
            .Where(e => (e.Id != tourExecutionId) && (_tourRepository.GetCountryByTourId(e.TourId) == country) &&
                        (_tourRepository.GetCityByTourId(e.TourId) == city))
            .Select(e => new TourDto
            {
                Id = e.Id,
                Name = _tourRepository.GetNameById(e.TourId),
                StartDate = e.StartDate,
                FirstPhoto = _tourRepository.GetPhotosById(e.TourId).FirstOrDefault()
            });
    }

    public bool CheckIfTourAlreadyReserved(int touristId, int tourExecutionId)
    {
        return _tourReservationService.GetAll()
            .Any(reservation => reservation.TouristId == touristId && reservation.TourExecutionId == tourExecutionId);
    }

    public TourDetailsDto GetTourDetailsDtos(int tourExecutionId)
    {
        TourExecution tourExecution = _tourExecutionService.GetById(tourExecutionId);
        Tour tour = _tourRepository.GetById(tourExecution.TourId);
        return new TourDetailsDto
        {
            Id = tourExecution.TourId,
            Name = tour.Name,
            StartDate = tourExecution.StartDate,
            Location = tour.Location,
            Language = tour.Language,
            MaxTouristNumber = tour.MaxTouristNumber,
            TourSpots = tour.TourSpots,
            Duration = tour.Duration,
            Photos = tour.Photos,
            Description = tour.Description
        };
    }

    public IEnumerable<TourDto> GetCancelableToursByTourGuideId(int tourGuideId)
    {
        return _tourExecutionService.GetAll().Where(e => e.StartDate > DateTime.Now.AddHours(48) &&
                                                         _tourRepository.GetTourGuideIdByTourId(e.TourId) ==
                                                         tourGuideId)
            .Select(e => new TourDto
                { Id = e.Id, Name = _tourRepository.GetNameById(e.TourId), StartDate = e.StartDate });
        ;
    }

    public List<NotificationsDto> GetNotificationDtos(int touristId)
    {
        List<Notification> notifications =
            _notificationService.GetAll().Where(e => e.TouristId == touristId).ToList();
        List<NotificationsDto> notificationsDtos = new List<NotificationsDto>();
        foreach (var notification in notifications)
        {
            NotificationsDto notificationDto = new NotificationsDto();
            notificationDto.Id = notification.Id;
            notificationDto.TourGuideId = notification.TourGuideId;
            notificationDto.SentDateTime = notification.SentDateTime;
            notificationDto.TourExecutionId = notification.TourExecutionId;
            notificationDto.TouristId = notification.TouristId;
            notificationDto.IsAccepted = notification.IsAccepted;
            notificationDto.IsSeen = notification.IsSeen;
            notificationDto.MessageContent = notification.MessageContent;
            notificationDto.TourGuideName = _userService.GetUsernameById(notification.TourGuideId);
            notificationDto.TourExecutionName =
                _tourRepository.GetNameById(
                    _tourExecutionService.GetTourIdByExecutionId(notification.TourExecutionId));
            notificationDto.TourExecutionDate =
                _tourExecutionService.GetById(notification.TourExecutionId).StartDate;
            notificationsDtos.Add(notificationDto);
        }

        return notificationsDtos;
    }

    public IEnumerable<TourExecution> GetAllFinished(int year = 0)
    {
        if (year == 0)
        {
            return _tourExecutionService.GetAll().Where(e => e.Status == Status.Finished);
        }
        else
        {
            return _tourExecutionService.GetAll().Where(e => e.Status == Status.Finished &&
                                                             e.StartDate.Year == year);
        }
    }
    public List<string> GetPhotosById(int id)
    {
        return _tourRepository.GetPhotosById(id);

    }

    
    public string GetNameById(int id)
    {
        return _tourRepository.GetNameById(id);

    }

    // prebacene iz tourReservation
    
    public Tour GetActiveTour(int touristId)
    {
        TourExecution tourExecution = _tourExecutionService.GetAll().Where(e => e.Status == Status.Started).FirstOrDefault();
        if (tourExecution != null)
        {
            TourReservation tourReservation = _tourReservationService.GetAll().Where(e => e.TouristId == touristId && e.TourExecutionId == tourExecution.Id).FirstOrDefault();
            if (tourReservation != null)
            {
                Tour tour = GetById(tourExecution.TourId);
                if (tour != null)
                {
                    return tour;
                }
            }
        }
        return null;
    }
    
    public string GetCurrentTourSpot(int touristId)
    {
        TourExecution tourExecution = _tourExecutionService.GetAll().Where(e => e.Status == Status.Started).FirstOrDefault();
        Tour tour = new Tour();
        if (tourExecution != null)
        {
            TourReservation tourReservation = _tourReservationService.GetAll().Where(e => e.TouristId == touristId && e.TourExecutionId == tourExecution.Id).FirstOrDefault();
            if (tourReservation != null)
            {
                tour = GetById(tourExecution.TourId);
                TourSpot currentTourSpot = tour.TourSpots.Where(e => e.Id == tourExecution.CurrentTourSpotId).FirstOrDefault();
                if (currentTourSpot != null)
                {
                    return currentTourSpot.Description;
                }
            }
        }
        return "";
    }
    
    public List<TourDto> GetAllFinishedTours(int touristId)
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
                    Name = GetNameById(tourExecution.TourId),
                    StartDate = tourExecution.StartDate
                };
                result.Add(tourDto);
            }
        }

        return result;
    }
    
    //            Name = _tourRepository.GetNameById(e.TourId),

    
    public List<TourDto> GetAllByTouristId(int touristId)
    {
        return _tourReservationService.GetAll().Where(e=>e.TouristId == touristId).Select(e => new TourDto
        {
            Id = e.Id,
            Name = _tourRepository.GetNameById(_tourExecutionService.GetTourIdByExecutionId(e.TourExecutionId)),
            StartDate = _tourExecutionService.GetById(e.TourExecutionId).StartDate,
            FirstPhoto = GetPhotosById(_tourExecutionService.GetTourIdByExecutionId(e.TourExecutionId)).FirstOrDefault()
        }).ToList();
    }

    public List<TourDto> GetAllByTouristIdReport(int touristId)
    {
        return _tourReservationService.GetAll().Where(e=>e.TouristId == touristId).Select(e => new TourDto
        {
            Id = e.Id,
            Name = _tourRepository.GetNameById(_tourExecutionService.GetTourIdByExecutionId(e.TourExecutionId)),
            StartDate = _tourExecutionService.GetById(e.TourExecutionId).StartDate,
            FirstPhoto = GetPhotosById(_tourExecutionService.GetTourIdByExecutionId(e.TourExecutionId)).FirstOrDefault(),
            OtherPeopleOnTour = e.OtherPeopleOnTour,
            Language = _tourRepository.GetById(_tourExecutionService.GetTourIdByExecutionId(e.TourExecutionId)).Language.ToString(),
            Country = _tourRepository.GetById(_tourExecutionService.GetTourIdByExecutionId(e.TourExecutionId)).Location.Country,
            City = _tourRepository.GetById(_tourExecutionService.GetTourIdByExecutionId(e.TourExecutionId)).Location.City,
            TourGuide = _userService.GetUsernameById(_tourRepository.GetById(_tourExecutionService.GetTourIdByExecutionId(e.TourExecutionId)).TourGuideId),
            Description = _tourRepository.GetById(_tourExecutionService.GetTourIdByExecutionId(e.TourExecutionId)).Description,
            Duration = _tourRepository.GetById(_tourExecutionService.GetTourIdByExecutionId(e.TourExecutionId)).Duration.ToString()
        }).ToList();
    }
    public void Delete(Tour tour)
    {
        _tourRepository.Delete(tour);
    }
    
}