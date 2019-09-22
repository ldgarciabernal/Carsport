namespace Carsport.Common.Models
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Message
    {
        public int MessageId { get; set; }

        [StringLength(128)]
        public string Sender { get; set; }

        public int ConversationId { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        [JsonIgnore]
        public virtual Conversation Conversation { get; set; }
    }
}
