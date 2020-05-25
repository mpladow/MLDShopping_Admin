using MLDShopping_Admin.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MLDShopping_Admin.Entities
{
    public class SubCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubCategoryId { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }//one-to-many
    }
}
