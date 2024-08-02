using System.Collections.Generic;
using System.IO;
using System.Linq;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;


namespace BookingApp.Repository;

public class ForumRepository: IForumRepository
{
    
    private const string FilePath = "../../../Resources/Data/forum.csv";

    private readonly Serializer<Forum> _serializer;

    private List<Forum> forums;
    public ForumRepository()
    {
        _serializer = new Serializer<Forum>();
        forums = _serializer.FromCSV(FilePath);
    }
    public List<Forum> GetAll()
    {
        return _serializer.FromCSV(FilePath);
    }

    public Forum Add(Forum forum)
    {
        forum.ForumId = NextID();
        forums = _serializer.FromCSV(FilePath);
        forums.Add(forum);
        _serializer.ToCSV(FilePath, forums);
        return forum;
    }

    public int NextID()
    {
        forums = _serializer.FromCSV(FilePath);
        if(forums.Count < 1)
        {
            return 1;
        }
        return forums.Max(c => c.ForumId) + 1;
    }

    public void Delete(Forum forum) 
    {
        forums = _serializer.FromCSV(FilePath);
        Forum founded = forums.Find(c => c.ForumId == forum.ForumId);
        forums.Remove(founded);
        _serializer.ToCSV(FilePath, forums);
    }

    public Forum? GetForumByID(int forumID)
    {
        forums = _serializer.FromCSV(FilePath);
        Forum founded = forums.Find(c => c.ForumId == forumID);
        return founded;
    }
    public Forum Update(Forum forum)
    {
        forums = _serializer.FromCSV(FilePath);
        Forum current = forums.Find(c => c.ForumId == forum.ForumId);
        int index = forums.IndexOf(current);
        forums.Remove(current);
        forums.Insert(index, forum);
        _serializer.ToCSV(FilePath, forums);
        return current;
    }
}