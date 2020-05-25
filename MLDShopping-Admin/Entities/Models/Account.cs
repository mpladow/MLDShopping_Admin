using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MLDShopping_Admin.Entities
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }
        [Required]
        [StringLength(255)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(255)]
        public string LastName { get; set; }
        public string Password { get; set; }
        public string ResetToken { get; set; }
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string UserImageUrl { get; set; }

        public virtual ICollection<AccountPermission> AccountPermissions { get; set; }
        public virtual ICollection<AccountPermission> Permissions { get; set; }


    }
}
