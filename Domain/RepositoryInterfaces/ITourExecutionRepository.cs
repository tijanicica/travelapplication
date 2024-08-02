using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.RepositoryInterfaces;

public interface ITourExecutionRepository
{
    TourExecution Save(TourExecution execution);


    int NextId();


    IEnumerable<TourExecution> GetAll();


    TourExecution GetById(int Id);


    IEnumerable<TourTourist> GetTourTouristsById(int id);


    int GetTourIdByExecutionId(int executionId);


    TourExecution Update(TourExecution tourExecution);


    void Delete(TourExecution tourExecution);


    TourTourist GetTourTouristByTouristIdAndExecutionId(int touristId, int executionId);
}