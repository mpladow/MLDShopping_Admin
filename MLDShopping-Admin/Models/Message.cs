using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MLDShopping_Admin.Models
{
    public enum MessageType
    {
        Danger = 1,
        Info = 2,
        Warning = 3,
        Success = 4
    }
    public class Message
    {
        public MessageType MessageType { get; set; }
        public string Text { get; set; }
    }
}
