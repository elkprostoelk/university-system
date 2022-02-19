﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversitySystem.Data.Repositories
{
    public interface IUserRepository
    {
        public Task<User> GetUser(string login);

        public Task<User> GetUser(int id);
        
        public Task AddUser(User user);
        
        public Task<bool> UserExists(string userName);
        public Task DeleteUser(User user);
    }
}
