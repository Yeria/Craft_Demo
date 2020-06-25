using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftBackEnd.Common.Models
{
    public class User : Person
    {
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        [MaxLength(5)]
        [Column(TypeName = "varchar(5)")]
        public string MemberType { get; set; }
        [Required]
        [MaxLength(5)]
        [Column(TypeName = "varchar(5)")]
        public string Status { get; set; }
        public bool IsAccountLocked { get; set; }
        public bool IsEmailVerified { get; set; }
        public Guid CredRef { get; set; }

        [NotMapped]
        public string Password { get; set; }
    }
}
