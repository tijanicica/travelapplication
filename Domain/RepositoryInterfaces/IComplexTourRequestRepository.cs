using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.RepositoryInterfaces;

public interface IComplexTourRequestRepository
{
    List<ComplexTourRequest> GetAll();
    ComplexTourRequest Save(ComplexTourRequest complexTourRequest);
    int NextId();
    void Delete(ComplexTourRequest complexTourRequest);
    ComplexTourRequest Update(ComplexTourRequest complexTourRequest);
}