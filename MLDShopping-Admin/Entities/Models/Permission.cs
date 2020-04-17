﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MLDShopping_Admin.Entities
{
    public class Permission
    {
        [Key]
        public int PermissionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
