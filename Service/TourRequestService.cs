using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.WPF.ViewModel.Tourist;

namespace BookingApp.Service;

public class TourRequestService : ITourRequestService
{
    private readonly ITourRequestRepository _tourRequestRepository;

    public TourRequestService(ITourRequestRepository tourRequestRepository)
    {
        _tourRequestRepository = tourRequestRepository;
    }
    
    public TourRequest Save(TourRequest tourRequest)
    {
        return _tourRequestRepository.Save(tourRequest);
    }

    public IEnumerable<TourRequest> GetAll()
    {
        return _tourRequestRepository.GetAll();
    }

    public List<TourRequest> GetAllByTouristId(int touristId)
    {
        return _tourRequestRepository.GetAll().Where(e=>e.TouristId == touristId).ToList();
    }
    
    public TourRequest Update(TourRequest tourRequest)
    {
        return _tourRequestRepository.Update(tourRequest);

    }

    public TourRequest GetById(int id)
    {
        return _tourRequestRepository.GetAll().Where(e => e.Id == id).FirstOrDefault();
    }


   

    public IEnumerable<TourRequest> GetByLocation(string location)
    {
        return _tourRequestRepository.GetAll()
            .Where(r => r.Location.City == location || r.Location.Country == location);
    }

    public IEnumerable<TourRequest> GetByLanguage(string language)
    {
        return _tourRequestRepository.GetAll()
            .Where(r => r.Language.ToString() == language);
    }

    public IEnumerable<TourRequest> GetByLocationAndYear(string location, int year)
    {
        return _tourRequestRepository.GetAll().Where(r =>
            (r.Location.City == location || r.Location.Country == location) && r.TourRequestDate.Year == year);
    }

    public IEnumerable<TourRequest> GetByLanguageAndYear(string language, int year)
    {
        return _tourRequestRepository.GetAll().Where(r =>
            r.Language.ToString() == language  && r.TourRequestDate.Year == year);
    }

    public string GetMostWantedLocation()
    {
        DateTime oneYearAgo = DateTime.Today.AddYears(-1);

        var locationGroups = _tourRequestRepository.GetAll()
            .Where(r => r.TourRequestDate >= oneYearAgo) 
            .GroupBy(r => r.Location.City)
            .Select(group => new
            {
                City = group.Key,
                Count = group.Count()
            })
            .OrderByDescending(x => x.Count);

        return locationGroups.FirstOrDefault()?.City;
    }

    public string GetMostWantedLanguage()
    {
        DateTime oneYearAgo = DateTime.Today.AddYears(-1);

        var locationGroups = _tourRequestRepository.GetAll()
            .Where(r => r.TourRequestDate >= oneYearAgo)
            .GroupBy(r => r.Language)
            .Select(group => new
            {
                Language = group.Key,
                Count = group.Count()
            })
            .OrderByDescending(x => x.Count);

        return locationGroups.FirstOrDefault()?.Language.ToString();
    }
   
    public double GetAcceptedRequestsPercentage(int touristId, string selectedYear)
    {
        var allTourRequests = GetAllByTouristId(touristId);

        // If selectedYear is not "All years", filter the requests by the specified year
        if (selectedYear != "All years")
        {
            allTourRequests = allTourRequests.Where(tr => tr.TourRequestDate.Year.ToString() == selectedYear).ToList();
        }

        // Calculate the number of accepted requests
        int tourRequestsAccepted = allTourRequests.Count(tr => tr.Status == TourRequestsStatus.Accepted);

        // Get the total number of requests
        int totalTourRequests = allTourRequests.Count;
        
        if (totalTourRequests == 0)
        {
            return 0.0; 
        }

        return (double)tourRequestsAccepted / totalTourRequests * 100; 
    }
    
    public double GetAveragePeopleInAcceptedRequests(int touristId, string selectedYear)
    {
        var requests = GetAllByTouristId(touristId).Where(r => r.Status == TourRequestsStatus.Accepted);

        if (selectedYear != "All years")
        {
            int year = int.Parse(selectedYear);
            requests = requests.Where(r => r.TourRequestDate.Year == year);
        }

        if (requests.Any())
        {
            return requests.Average(r => r.PeopleOnTour.Count);
        }
        return 0;
    }

    public List<LanguageStatisticModelViewModel> GetRequestCountsByLanguage()
    {
        return _tourRequestRepository.GetAll()
            .GroupBy(request => request.Language)
            .Select(group => new LanguageStatisticModelViewModel()
            {
                Language = group.Key.ToString(),
                Count = group.Count()
            })
            .ToList();
    }
    
    public List<LocationStatisticModelViewModel> GetRequestCountsByLocation()
    {
        return _tourRequestRepository.GetAll()
            .GroupBy(request => request.Location.City)  // Assuming Location has a City property
            .Select(group => new LocationStatisticModelViewModel()
            {
                LocationName = group.Key,
                Count = group.Count()
            })
            .ToList();
    }
    
    public List<TourRequest> GetAllNewTours(int touristId)
    {
        // Get all unfulfilled requests for the tourist
        var unfulfilledRequests = _tourRequestRepository.GetAll()
            .Where(tr => tr.TouristId == touristId && tr.Status == TourRequestsStatus.Pending)
            .ToList();

        // Extract languages and locations from unfulfilled requests
        var requestedLanguages = unfulfilledRequests.Select(req => req.Language).Distinct();
        var requestedCities = unfulfilledRequests.Select(req => req.Location.City).Distinct();

        // Get all new tours that are accepted and not created by the tourist
        var newTours = _tourRequestRepository.GetAll()
            .Where(tr => tr.Status == TourRequestsStatus.Accepted && tr.TouristId != touristId)
            .ToList();

        // Filter new tours by requested languages and locations
        var relevantTours = newTours.Where(tour =>
                requestedLanguages.Contains(tour.Language) ||
                requestedCities.Contains(tour.Location.City))
            .ToList();

        return relevantTours;
    }
    




}