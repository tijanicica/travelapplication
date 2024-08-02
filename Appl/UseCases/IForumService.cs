using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Appl.UseCases;

public interface IForumService
{
    public void Update(Forum forum);

    public List<Forum> GetAll();
    public List<Forum> GetUnseenForums(int ownerID);

    public bool doesOwnerHaveAccommodationAtForumsLocation(Forum f, int ownerID);
}