using System.Collections.Generic;
using BookingApp.Domain.Model;
using BookingApp.Dto;
using BookingApp.Parameters;

namespace BookingApp.Appl.UseCases;

public interface ITourService


{
    void Delete(Tour tour);
    string GetNameById(int id);
    List<string> GetPhotosById(int id);
   // Tour GetMostVisited(int year = 0);
    IEnumerable<TourExecution> GetAllFinished(int year = 0);
    IEnumerable<TourDto> GetFilteredTours(FilterTours filter);


    IEnumerable<TourDto> GetTourDtos();


    IEnumerable<TourDto> GetToursTodayByTourGuideId(int tourGuideId);


    int GetTourIdByExecutionId(int executionId);


    Tour GetById(int id);


    Tour Update(Tour tour);


    Tour Save(Tour tour);


    IEnumerable<TourDto> GetAll();


    IEnumerable<TourDto> GetByCountry(string country);


    IEnumerable<TourDto> GetByCity(string city);


    IEnumerable<TourDto> GetByDuration(double duration);


    IEnumerable<TourDto> GetByMaxTouristNumber(int touristNumber);


    IEnumerable<TourDto> GetByLanguage(Language language);


    bool IsBelowMaxCapacity(int touristsNumbers, int tourExecutionId);


    bool IsFull(int tourExecutionId);


    int GetCurrentFreeSpots(int tourExecutionId);


    IEnumerable<Tour> GetByTourGuideId(int tourGuideId);


    IEnumerable<TourSpot> GetTourSpotsByTourId(int tourId);


    IEnumerable<TourDto> GetTourDtosByLocation(int tourExecutionId);


    bool CheckIfTourAlreadyReserved(int touristId, int tourExecutionId);


    TourDetailsDto GetTourDetailsDtos(int tourExecutionId);


    IEnumerable<TourDto> GetCancelableToursByTourGuideId(int tourGuideId);


    List<NotificationsDto> GetNotificationDtos(int touristId);
    //prebacene iz TourReservation 
    List<TourDto> GetAllByTouristId(int touristId);
    List<TourDto> GetAllFinishedTours(int touristId);
    string GetCurrentTourSpot(int touristId);
    Tour GetActiveTour(int touristId);

    List<TourDto> GetAllByTouristIdReport(int touristId);



    /*int CalculateChildrenVisitors(int tourId);
    int CalculateAdultVisitors(int tourId);
    int CalculateElderlyVisitors(int tourId);*/

    //TourExecution AddTouristToTourExecution(TourTourist tourist, int tourExecutionId);

}