using System.Collections.Generic;
using System.Linq;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.Service;

public class ComplexTourRequestService : IComplexTourRequestService
{
    private readonly IComplexTourRequestRepository _complexTourRequestRepository;

    public ComplexTourRequestService(IComplexTourRequestRepository complexTourRequestRepository)
    {
        _complexTourRequestRepository = complexTourRequestRepository;
    }

    public ComplexTourRequest Save(ComplexTourRequest complexTourRequest)
    {
        return _complexTourRequestRepository.Save(complexTourRequest);
    }

    public List<ComplexTourRequest> GetAll()
    {
        return _complexTourRequestRepository.GetAll();
    }

    public ComplexTourRequest GetById(int id)
    {
        return _complexTourRequestRepository.GetAll().Where(e => e.Id == id).FirstOrDefault();
    }
}