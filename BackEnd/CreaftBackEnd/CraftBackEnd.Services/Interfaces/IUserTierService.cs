using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CraftBackEnd.Services.Interfaces
{
    public interface IUserTierService
    {
        Task<int> GetFieldCountLimitAsync();
    }
}
