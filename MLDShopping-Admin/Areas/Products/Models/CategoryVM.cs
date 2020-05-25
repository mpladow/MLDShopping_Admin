using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using MLDShopping_Admin.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MLDShopping_Admin.Models
{
    public class CategoryVM
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? DeletedAt { get; set; }
        //public List<int> SubCategoryIds { get; set; }
        //public List<SubCategoryVM> SubCategories { get; set; }

    }
}
