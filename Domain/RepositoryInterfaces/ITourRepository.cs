using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.RepositoryInterfaces;

public interface ITourRepository


{

    void Delete(Tour tour);
    string GetNameById(int id);

    List<string> GetPhotosById(int id);


    string GetCountryByTourId(int id);


    string GetCityByTourId(int id);


    double GetDurationByTourId(int id);


    int GetMaxTouristNumberByTourId(int id);


    Language GetLanguageByTourId(int id);


    Tour Save(Tour tour);


    int NextId();

    int GetTourGuideIdByTourId(int tourId);


    IEnumerable<Tour> GetAll();


    Tour GetById(int Id);


    Tour Update(Tour tour);
}