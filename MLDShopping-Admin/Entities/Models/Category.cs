using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace MLDShopping_Admin.Entities.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool? DeletedAt { get; set; }

        public virtual ICollection<Product> Products { get; set; } // many products to one cateogy
        //public virtual ICollection<SubCategory> SubCategories{ get; set; } // one category to many subcategories

    }
}
