using System.Collections.Generic;
using System.Linq;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;

namespace BookingApp.Repository;

public class TourExecutionRepository : ITourExecutionRepository
{
    private const string FilePath = "../../../Resources/Data/tourExecutions.csv";
    
    private readonly Serializer<TourExecution> _serializer;
    
    private List<TourExecution> _executions;
    
    public TourExecutionRepository()
    {
        _serializer = new Serializer<TourExecution>();
        _executions = _serializer.FromCSV(FilePath);
    }

    
    
    public TourExecution Save(TourExecution execution)
    {
        execution.Id = NextId();
        _executions = _serializer.FromCSV(FilePath);
        _executions.Add(execution);
        _serializer.ToCSV(FilePath, _executions);
        return execution;
    }
 
    public int NextId()
    {
        _executions = _serializer.FromCSV(FilePath);
        if (_executions.Count < 1)
        {
            return 1;
        }
        return _executions.Max(c => c.Id) + 1;
    }

    
    public IEnumerable<TourExecution> GetAll()
    {
        UpdateList();
        return _executions;
    }

    private void UpdateList()
    {
        _executions = _serializer.FromCSV(FilePath);
    }
    public TourExecution GetById(int Id)
    {
        UpdateList();
        var execution = _executions.FirstOrDefault(t => t.Id == Id);
        return execution;
    }

    public IEnumerable<TourTourist> GetTourTouristsById(int id)
    {
        return GetById(id).Tourists;
    } 

    
    public int GetTourIdByExecutionId(int executionId)
    {
        var execution = GetById(executionId);
        if (execution != null)
        {
            return execution.TourId;
                    
        }

        return -1;
    }

    
    public TourExecution Update(TourExecution tourExecution)
    {
        _executions = _serializer.FromCSV(FilePath);
        TourExecution current = _executions.Find(c => c.Id == tourExecution.Id);
        int index = _executions.IndexOf(current);
        _executions.Remove(current);
        _executions.Insert(index, tourExecution);      
        _serializer.ToCSV(FilePath, _executions);
        return tourExecution;
    }
    
    public void Delete(TourExecution tourExecution)
    {
        _executions = _serializer.FromCSV(FilePath);
        TourExecution founded = _executions.Find(c => c.Id == tourExecution.Id);
        _executions.Remove(founded);
        _serializer.ToCSV(FilePath, _executions);
    }

    public TourTourist GetTourTouristByTouristIdAndExecutionId(int touristId, int executionId)
    {
        var execution = _executions.FirstOrDefault(e => e.Id == executionId);
        if (execution != null)
        {
            return execution.Tourists.FirstOrDefault(t => t.TouristId == touristId);
        }
        return null;
    }


}