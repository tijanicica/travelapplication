using System.Collections.Generic;
using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Tourist;


namespace BookingApp.Appl.UseCases;

public interface ITourRequestService
{
    TourRequest Save(TourRequest tourRequest);

    
    IEnumerable<TourRequest> GetAll();

   
    

    IEnumerable<TourRequest> GetByLocation(string location);
    IEnumerable<TourRequest> GetByLanguage(string language);
    IEnumerable<TourRequest> GetByLocationAndYear(string location, int year);

    IEnumerable<TourRequest> GetByLanguageAndYear(string language, int year);

    string GetMostWantedLocation();

    string GetMostWantedLanguage();

    List<TourRequest> GetAllByTouristId(int touristId);
    TourRequest Update(TourRequest tourRequest);
    TourRequest GetById(int id);
    double GetAcceptedRequestsPercentage(int touristId, string selectedYear);
    double GetAveragePeopleInAcceptedRequests(int touristId, string selectedYear);
    List<LanguageStatisticModelViewModel> GetRequestCountsByLanguage();
    List<LocationStatisticModelViewModel> GetRequestCountsByLocation();
    List<TourRequest> GetAllNewTours(int touristId);

}