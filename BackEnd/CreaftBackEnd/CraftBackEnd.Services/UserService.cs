using CraftBackEnd.Common.Models;
using CraftBackEnd.Common.Models.Type;
using CraftBackEnd.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CraftBackEnd.Services
{
    public class UserService : IUserService
    {
        private readonly IDatabaseContext _dbContext;
        public UserService(IDatabaseContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<User> GetUser(int userId) {
            return await _dbContext.User.FindAsync(userId);
        }

        public bool UpdateUser(User user) {
            throw new NotImplementedException();
        }

        public bool DeleteUser(int userId) {
            throw new NotImplementedException();
        }
    }
}
