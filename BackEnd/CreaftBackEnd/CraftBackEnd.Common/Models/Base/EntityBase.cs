using System;
using System.Collections.Generic;
using System.Text;

namespace CraftBackEnd.Common.Models.Base
{
    public class EntityBase
    {
        public int Id { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
