using BookingApp.Domain.Model;

namespace BookingApp.Domain.RepositoryInterfaces;

public interface IUserRepository
{
    User GetByUsername(string username);


    User GetById(int id);


    string GetUsernameById(int id);

    void Delete(User user);

}