using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Community.Models
{
    public class Message
    {
        [Key]
        public string Id { get; set; }
        [Required, MinLength(1)]
        public string Subject { get; set; }
        [Required, MinLength(1)]
        public string Content { get; set; }
        [Required]
        public DateTime SendingDate { get; set; }
        [Required]
        public string Sender { get; set; }
        [Required]
        public string Recipient { get; set; }
        [Required]
        public bool Seen { get; set; }
        [Required]
        public bool Deleted { get; set; }
        private Message() { }
        public Message(string id, string subject, string content, DateTime sendingDate, string sender, string recipient, bool seen, bool deleted)
        {
            Id = id;
            Subject = subject;
            Content = content;
            SendingDate = sendingDate;
            Sender = sender;
            Recipient = recipient;
            Seen = seen;
            Deleted = deleted;
        }
    }
}
