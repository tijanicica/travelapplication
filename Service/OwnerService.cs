using System.Collections.Generic;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repository;

namespace BookingApp.Service;

public class OwnerService : IOwnerService
{
    private readonly IOwnerRepository _ownerRepository;

    public OwnerService(IOwnerRepository ownerRepository)
    {
        _ownerRepository = ownerRepository;
    }

    public void UpdateIsSuperOwner(int ownerId, bool isSuperOwner)
    {
        _ownerRepository.UpdateIsSuperOwner(ownerId, isSuperOwner);
    }

    public List<Owner> GetAll()
    {
       return _ownerRepository.GetAll();
    }
    
    public Owner GetById(int id)
    {
        return _ownerRepository.GetById(id);
    }
}