using System.Collections.Generic;
using BookingApp.Appl.UseCases;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.Service;

public class ForumService: IForumService
{
    private readonly  IForumRepository _forumRepository;
    private IAccommodationService _accommodationService;
    private readonly IUserService _userService;
  

    public ForumService(IForumRepository forumRepository,
        IUserService userService, IAccommodationService accommodationService)
    {
        _forumRepository = forumRepository;
        _userService = userService;
        _accommodationService = accommodationService;
      

    }
    public void Update(Forum forum)
    {
        _forumRepository.Update(forum);
    }

    public List<Forum> GetAll()
    {
        List<Forum> forums = new List<Forum>();
        foreach(var f in _forumRepository.GetAll())
        {
           // f.location = _locationService.GetLocationByLocationID(f.locationID);
            forums.Add(f);
        }
        return forums;
    }
    public List<Forum> GetUnseenForums(int ownerID)
    {
        List<Forum> list = new List<Forum> ();
        foreach(Forum f in _forumRepository.GetAll())
        {
            if(doesOwnerHaveAccommodationAtForumsLocation(f, ownerID) && f.HasOwnerSeenTheNotification==false)
            {
              //  f.location = _locationService.GetLocationByLocationID(f.locationID);
                list.Add(f);
            }
        }
        return list;
    }

    public bool doesOwnerHaveAccommodationAtForumsLocation(Forum f, int ownerID)
    {
        List<Accommodation> accommodationsByOwnerID = new List<Accommodation>(_accommodationService.GetAllByOwnerID(ownerID));
        foreach(Accommodation a in accommodationsByOwnerID)
        {
            if(a.Location == f.Location)
            {
                return true;
            }
        }
        return false;
    }

}