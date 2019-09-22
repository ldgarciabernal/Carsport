using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carsport.Common.Models
{
    public class ConversationRequest
    {
        public string UserIdOne { get; set; }

        public string UserIdTwo { get; set; }

        public DateTime Date { get; set; }

        public int RouteId { get; set; }

        public string ConnectionId { get; set; }
    }
}
