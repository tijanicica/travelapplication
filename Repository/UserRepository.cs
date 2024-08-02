using BookingApp.Serializer;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Domain.Model;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.Repository
{
    public class UserRepository : IUserRepository
    {
        private const string FilePath = "../../../Resources/Data/users.csv";

        private readonly Serializer<User> _serializer;

        private List<User> _users;

        public UserRepository()
        {
            _serializer = new Serializer<User>();
            _users = _serializer.FromCSV(FilePath);
        }

        public User GetByUsername(string username)
        {
            _users = _serializer.FromCSV(FilePath);
            return _users.FirstOrDefault(u => u.Username == username);
        }
        
        public User GetById(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            return user;
        }

        public string GetUsernameById(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            return user.Username;
           
        }
        
        public void Delete(User user)
        {
            _users = _serializer.FromCSV(FilePath);
            User founded = _users.Find(c => c.Id == user.Id);
            _users.Remove(founded);
            _serializer.ToCSV(FilePath, _users);
        }
    }
}
