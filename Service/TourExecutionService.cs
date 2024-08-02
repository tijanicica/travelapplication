using System.Collections.Generic;
using System.Linq;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Dto;
using BookingApp.Repository;

namespace BookingApp.Service;

public class TourExecutionService : ITourExecutionService
{
     private readonly ITourExecutionRepository _tourExecutionRepository;
     

    

    public TourExecutionService(ITourExecutionRepository tourExecutionRepository)
    {
        _tourExecutionRepository = tourExecutionRepository;
        
    }
    public IEnumerable<TourExecution> GetAll()
    {
        return _tourExecutionRepository.GetAll();
    }
    

    public TourExecution GetById(int id)
    {
        return _tourExecutionRepository.GetById(id);
    }

    public TourExecution Update(TourExecution tourExecution)
    {
        return _tourExecutionRepository.Update(tourExecution);
    }
    
    public TourExecution Save(TourExecution tourExecution)
    {
        return _tourExecutionRepository.Save(tourExecution);
    }

    public List<TourExecution> GetTourExecutions(int tourId)
    {
        return _tourExecutionRepository.GetAll().Where(execution => execution.TourId == tourId).ToList();
    }
    
    public IEnumerable<TourExecution> GetAllTourExecutions()
    {
        return _tourExecutionRepository.GetAll();
    }
    
    public IEnumerable<TourTourist> GetTourTouristsById(int id)
    {
        //return GetById(id).Tourists;
        return _tourExecutionRepository.GetTourTouristsById(id);
    }
    
    public TourExecution AddTouristToTourExecution(TourTourist tourist, int tourExecutionId)
    {

        TourExecution tourExecution = _tourExecutionRepository.GetById(tourExecutionId);
        tourExecution.Tourists.Add(tourist);
        return _tourExecutionRepository.Update(tourExecution);
    }
    
    public void Delete(TourExecution tourExecution)
    {
       _tourExecutionRepository.Delete(tourExecution);
    }
    
    public TourTourist GetTourTouristByTouristIdAndExecutionId(int touristId, int executionId)
    {
        return _tourExecutionRepository.GetTourTouristByTouristIdAndExecutionId(touristId, executionId);
    }
    
    public int GetTourIdByExecutionId(int executionId)
    {
        return _tourExecutionRepository.GetTourIdByExecutionId(executionId);
    }

    public IEnumerable<TourExecution> GetByTourId(int tourId)
    {
        return _tourExecutionRepository.GetAll().Where(e => e.TourId == tourId);
    }

   
   
}