using BookingApp.Domain.Model;

namespace BookingApp.Appl.UseCases;

public interface IComplexTourPartService
{
    ComplexTourPart Save(ComplexTourPart complexTourPart);
    ComplexTourPart GetById(int complexTourPartId);
    ComplexTourPart Update(ComplexTourPart complexTourPart);
}