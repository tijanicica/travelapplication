using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.RepositoryInterfaces;

public interface IComplexTourPartRepository
{
    List<ComplexTourPart> GetAll();
    ComplexTourPart Save(ComplexTourPart complexTourPart);
    int NextId();
    void Delete(ComplexTourPart complexTourPart);
    ComplexTourPart Update(ComplexTourPart complexTourPart);
    
}