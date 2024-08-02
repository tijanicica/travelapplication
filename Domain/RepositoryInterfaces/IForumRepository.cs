using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.RepositoryInterfaces;

public interface IForumRepository
{
    public List<Forum> GetAll();
    public Forum Add(Forum forum);
    public void Delete(Forum forum);
    public Forum? GetForumByID(int forumID);
    public Forum Update(Forum forum);
}