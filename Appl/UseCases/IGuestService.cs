using System.Collections.Generic;
using BookingApp.Domain.Model;
using BookingApp.Dto;

namespace BookingApp.Appl.UseCases;

public interface IGuestService
{ 
    Guest Save(Guest guest);
    Guest GetById(int id);
    IEnumerable<GuestDto> GetAll();
    IEnumerable<GuestDto> GetGuestDtos();
}