using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MLDShopping_Admin.Entities
{
    public class MediaUrl
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MediaUrlId { get; set; }
        [Required]
        [StringLength(255)]
        public string MediaPath { get; set; }
        public bool? IsImage { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product{ get; set; }
    }
}
