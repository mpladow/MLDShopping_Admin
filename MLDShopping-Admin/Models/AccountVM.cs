using Microsoft.AspNetCore.Mvc.Rendering;
using MLDShopping_Admin.Entities;
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
        public string Password { get; set; }
        public string CreatedAt { get; set; }
        public bool Deleted { get; set; }
        public string PermissionsString { get; set; }
        public List<PermissionVM> Permissions { get; set; } = new List<PermissionVM>();
        public List<string> PermissionIds{ get; set; } = new List<string>();
        public List<SelectListItem> SelectList { get; set; } = new List<SelectListItem>();


    }
}
