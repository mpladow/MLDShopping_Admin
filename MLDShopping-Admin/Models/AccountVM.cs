using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MLDShopping_Admin.Models
{
    public class AccountVM
    {
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public string CreatedAt { get; set; }
        public bool Deleted { get; set; }
        public string Permissions { get; set; }

    }
}
