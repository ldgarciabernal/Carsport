using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carsport.Common.Models
{
    public class ConversationUser
    {
        public int ConversationId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
        
        public ICollection<Message> Messages { get; set; }

        public string UserIdOne { get; set; }

        public string UserIdTwo { get; set; }
    }
}
