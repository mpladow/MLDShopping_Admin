using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using MLDShopping_Admin.Entities;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MLDShopping_Admin.Models
{
    public class ProductVM
    {
        public int ProductId { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public string ProductCode { get; set; }
        [Required]
        public string Description1 { get; set; }
        [Required]
        public string Description2 { get; set; }
        public bool IsActive { get; set; }
        public string DeletedAt { get; set; }
        public string AddedAt { get; set; }
        public int CategoryId { get; set; }
        //public int SubCategoryId { get; set; }

        public CategoryVM Category { get; set; } //one to many
        //public SubCategoryVM SubCategory { get; set; }
        public List<string> MediaUrls { get; set; }//one-to-many
        public List<IFormFile> MediaFiles { get; set; }//one-to-many

    }
}
