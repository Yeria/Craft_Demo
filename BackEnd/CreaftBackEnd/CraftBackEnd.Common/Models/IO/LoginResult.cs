using System;
using System.Collections.Generic;
using System.Text;

namespace CraftBackEnd.Common.Models.IO
{
    public class LoginResult
    {
        public User User { get; set; }
        public string Token { get; set; }
        //public List<UserRole> Roles { get; set; }
    }
}
