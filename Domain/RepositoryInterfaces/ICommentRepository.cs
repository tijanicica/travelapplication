using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.RepositoryInterfaces;

public interface ICommentRepository
{ 
        List<Comment> GetAll();
        Comment Save(Comment comment);
        int NextId();
        void Delete(Comment comment);
        Comment Update(Comment comment);
        List<Comment> GetByUser(User user);
}