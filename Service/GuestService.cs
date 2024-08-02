using System.Collections.Generic;
using System.Linq;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Dto;
using BookingApp.Repository;

namespace BookingApp.Service;

public class GuestService : IGuestService
{
    private readonly IUserService _userService;
    private readonly IGuestRepository _guestRepository;
  
    public GuestService(IGuestRepository guestRepository)
    {
        _guestRepository = guestRepository;
        
    }
    
    public Guest Save(Guest guest)
    {
        return _guestRepository.Save(guest);
    }
 
    public Guest GetById(int id)
    {
        return _guestRepository.GetById(id);
    }
  
    public IEnumerable<GuestDto> GetAll()
    {
        return _guestRepository.GetAll().Select(e => new GuestDto
        {
            Id = e.Id,
            Username = e.Username,
        });
    }
  
    public IEnumerable<GuestDto> GetGuestDtos()
    {
        return _guestRepository.GetAll().Select(e => new GuestDto
        {
            
            Id = e.Id,
            Username = e.Username,
            
        });
    }
    
    
}