using BookingApp.Domain.Model;

namespace BookingApp.Domain.RepositoryInterfaces;

public interface ITouristRepository
{
    User GetByUsername(string username);

}