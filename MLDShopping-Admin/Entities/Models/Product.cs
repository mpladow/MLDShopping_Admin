using MLDShopping_Admin.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MLDShopping_Admin.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        [StringLength(255)]
        public string ProductCode { get; set; }
        [Required]
        public string Description1 { get; set; }
        [Required]
        public string Description2 { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? AddedAt { get; set; }
        public int CategoryId{ get; set; }
        public int? SubCategoryId { get; set; }

        public virtual ICollection<MediaUrl> MediaUrls { get; set; }//one-to-many
        public virtual Category Category { get; set; } //One to many
        //public virtual SubCategory SubCategory { get; set; } //Many to one


    }
}
