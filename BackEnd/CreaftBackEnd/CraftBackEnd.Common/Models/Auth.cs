using System;
using System.Collections.Generic;
using System.Text;

namespace CraftBackEnd.Common.Models
{
    public class Auth
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public Guid CredRef { get; set; }
        public string Salt { get; set; }
    }
}
