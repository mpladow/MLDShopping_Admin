using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MLDShopping_Admin.Models
{
    public class ProductVM
    {
        public int ProductId { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
    }
}
