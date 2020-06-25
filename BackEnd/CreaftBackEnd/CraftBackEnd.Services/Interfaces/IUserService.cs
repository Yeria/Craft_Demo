using CraftBackEnd.Common.Models;
using CraftBackEnd.Common.Models.Type;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CraftBackEnd.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUser(int id);
    }
}
