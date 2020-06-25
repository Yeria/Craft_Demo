using CraftBackEnd.Common.Models;
using CraftBackEnd.Common.Models.IO;
using CraftBackEnd.Common.Models.Type;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CraftBackEnd.Services.Interfaces
{
    public interface IIAMService
    {
        int GetReferenceId();
        Task<IList<UserRole>> GetUserRolesAsync(int userId);
        Task<LoginResult> LoginAsync(string userName, string password);
        Task<bool> CreateAccountAsync(User newUser);
        Task<string> IsAuthenticatedAsync();
    }
}
