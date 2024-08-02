using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Appl.UseCases;

public interface IComplexTourRequestService
{
    ComplexTourRequest Save(ComplexTourRequest complexTourRequest);
    List<ComplexTourRequest> GetAll();
    ComplexTourRequest GetById(int id);

}