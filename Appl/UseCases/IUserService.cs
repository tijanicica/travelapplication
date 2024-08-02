using BookingApp.Domain.Model;

namespace BookingApp.Appl.UseCases;

public interface IUserService
{
    string GetUsernameById(int id);


    User GetById(int id);

    void Delete(User user);

}