namespace Carsport.Common.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Conversation
    {
        public int ConversationId { get; set; }

        [StringLength(128)]
        public string UserIdOne { get; set; }

        [StringLength(128)]
        public string UserIdTwo { get; set; }

        [Display(Name = "Departure date")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        public string ConnectionId { get; set; }

        [JsonIgnore]
        public virtual ICollection<Route> Routes { get; set; }

        public virtual ICollection<Message> Messages {get; set;}
    }
}
