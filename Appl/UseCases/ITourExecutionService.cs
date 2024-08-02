using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Appl.UseCases;

public interface ITourExecutionService
{
    IEnumerable<TourExecution> GetAll();



    TourExecution GetById(int id);

    TourExecution Update(TourExecution tourExecution);


    TourExecution Save(TourExecution tourExecution);


    List<TourExecution> GetTourExecutions(int tourId);


    IEnumerable<TourExecution> GetAllTourExecutions();


    IEnumerable<TourTourist> GetTourTouristsById(int id);


    TourExecution AddTouristToTourExecution(TourTourist tourist, int tourExecutionId);


    void Delete(TourExecution tourExecution);


    TourTourist GetTourTouristByTouristIdAndExecutionId(int touristId, int executionId);
    int GetTourIdByExecutionId(int executionId);
     IEnumerable<TourExecution> GetByTourId(int tourId);

}