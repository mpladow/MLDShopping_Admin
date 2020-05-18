using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MLDShopping_Admin.Models
{
    public class ChatVM
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public List<string> Messages { get; set; }
    }
}
