using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MLDShopping_Admin.Entities
{
    public class AccountPermission
    {
        [Key]
        public int AccountPermissionId { get; set; }
        public int AccountId { get; set; }
        public string PermissionId { get; set; }
        public virtual Account Account { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
