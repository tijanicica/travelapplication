using System.Linq;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.Service;

public class ComplexTourPartService : IComplexTourPartService
{
    private readonly IComplexTourPartRepository _complexTourPartRepository;

    public ComplexTourPartService(IComplexTourPartRepository complexTourPartRepository)
    {
        _complexTourPartRepository = complexTourPartRepository;
    }
    
    public ComplexTourPart Save(ComplexTourPart complexTourPart)
    {
        return _complexTourPartRepository.Save(complexTourPart);
    }

    public ComplexTourPart GetById(int complexTourPartId)
    {
        return _complexTourPartRepository.GetAll().Where(e => e.Id == complexTourPartId).FirstOrDefault();
    }

    public ComplexTourPart Update(ComplexTourPart complexTourPart)
    {
        return _complexTourPartRepository.Update(complexTourPart);
    }
}