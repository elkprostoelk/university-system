using System.Collections.Generic;
using System.Threading.Tasks;
using UniversitySystem.Data.Entities;

namespace UniversitySystem.Data.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> GetUser(string login);

        public Task<User> GetUser(long id);
        
        public Task AddUser(User user);
        
        public Task<bool> UserExists(string userName);
        public Task DeleteUser(User user);
        public Task UpdateUser(User user);
        public Task<ICollection<User>> GetAllUsers();
    }
}
