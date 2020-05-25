using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using MLDShopping_Admin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MLDShopping_Admin.Models
{
    public class SubCategoryVM
    {
        public int SubCategoryId { get; set; }
        public string Name { get; set; }
        public bool? DeletedAt { get; set; }
        public int CategoryId { get; set; }
        public CategoryVM Category{ get; set; }
    }
}
