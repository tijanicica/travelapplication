using System;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;

namespace BookingApp.Service;

public class  TourStatisticsService : ITourStatisticsService
{
    
    private readonly ITourReservationService _tourReservationService;
    private readonly ITourService _tourService;
    public TourStatisticsService(ITourService tourService, 
        ITourReservationService tourReservationService)
    {
        _tourService = tourService;
        _tourReservationService = tourReservationService;
        
    }
    
    
      public Tour GetMostVisited(int year = 0)
    {
        // koji je tour id i koliko je ljudi doslo 
        Dictionary<int, int> numberOfVisitors = new Dictionary<int, int>();

        if (! _tourService.GetAllFinished(year).Any())
        {
            throw new Exception($"No finished tours found for the year {year}.");
        }

        // ever

        foreach (var execution in _tourService.GetAllFinished(year))
        {
            if (numberOfVisitors.Keys.Contains(execution.TourId))
            {
                numberOfVisitors[execution.TourId] +=
                    execution.Tourists.Count(tourist => tourist.JoinedAtTourSpot != -1);
            }
            else
            {
                numberOfVisitors[execution.TourId] =
                    execution.Tourists.Count(tourist => tourist.JoinedAtTourSpot != -1);
            }

            foreach (var reservation in _tourReservationService.GetAll()
                         .Where(e => e.TourExecutionId == execution.Id))
            {
                if (numberOfVisitors.Keys.Contains(execution.TourId))
                {
                    numberOfVisitors[execution.TourId] +=
                        reservation.OtherPeopleOnTour.Count(person => person.Arrived);
                }
                else
                {
                    numberOfVisitors[execution.TourId] =
                        reservation.OtherPeopleOnTour.Count(person => person.Arrived);
                }
            }
        }

        var sortedDict = numberOfVisitors.OrderBy(pair => pair.Value);
        return _tourService.GetById(sortedDict.Last().Key);
    }

    public int CalculateChildrenVisitors(int tourId)
    {
        int numberOfChildren = 0;
        foreach (var execution in _tourService.GetAllFinished().Where(e => e.TourId == tourId))
        {
            foreach (var reservation in _tourReservationService.GetAll()
                         .Where(e => e.TourExecutionId == execution.Id))
            {
                foreach (var person in reservation.OtherPeopleOnTour)
                {
                    if (person.Age < 18 && person.Arrived)
                    {
                        numberOfChildren++;
                    }
                }
            }
        }

        return numberOfChildren;
    }

    public int CalculateAdultVisitors(int tourId)
    {
        int numberOfAdults = 0;
        foreach (var execution in _tourService.GetAllFinished().Where(e => e.TourId == tourId))
        {
            foreach (var reservation in _tourReservationService.GetAll()
                         .Where(e => e.TourExecutionId == execution.Id))
            {
                foreach (var person in reservation.OtherPeopleOnTour)
                {
                    if (person.Age >= 18 && person.Age <= 50  && person.Arrived)
                    {
                        numberOfAdults++;
                    }
                }
            }
        }

        return numberOfAdults;
    }

    public int CalculateElderlyVisitors(int tourId)
    {
        int numberOfElderly = 0;
        foreach (var execution in _tourService.GetAllFinished().Where(e => e.TourId == tourId))
        {
            foreach (var reservation in _tourReservationService.GetAll()
                         .Where(e => e.TourExecutionId == execution.Id))
            {
                foreach (var person in reservation.OtherPeopleOnTour)
                {
                    if (person.Age > 50 && person.Arrived)
                    {
                        numberOfElderly++;
                    }
                }
            }
        }

        return numberOfElderly;
    }
}