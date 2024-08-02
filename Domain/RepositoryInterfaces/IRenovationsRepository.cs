using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.RepositoryInterfaces;

public interface IRenovationsRepository
{
    public List<Renovations> GetAll();
    public Renovations Save(Renovations accommodation);
    public int NextId();
    public void Delete(Renovations accommodation);
    public Renovations? removeRenovation(int idAcm);
    public Renovations? getRenovationByID(int idAcm);
    public Renovations Update(Renovations accommodation);
}