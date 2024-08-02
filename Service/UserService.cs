using BookingApp.Repository;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repository;

namespace BookingApp.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
   
    
    

    public UserService(IUserRepository userRepository )
    {
        _userRepository = userRepository;

    }
    
    public string GetUsernameById(int id)
    {
        return _userRepository.GetUsernameById(id);
    }
    
    public User GetById(int id)
    {
        return _userRepository.GetById(id);
    }

    public void Delete(User user)
    {
        _userRepository.Delete(user);
    }
}