using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Community.ViewModels
{
    public class HomeRecivedMessagesViewModel
    {
        public string Id { get; set; }
        public string SenderId { get; set; }
        public string SenderEmail { get; set; }
        public string Subject { get; set; }
        public string SendingDate { get; set; }
        [Required]
        public bool Seen { get; set; }
        [Required]
        public bool Deleted { get; set; }

        public HomeRecivedMessagesViewModel(string id, string senderId, string senderEmail, string subject, string sendingDate, bool seen, bool deleted)
        {
            Id = id;
            SenderId = senderId;
            SenderEmail = senderEmail;
            Subject = subject;
            SendingDate = sendingDate;
            Seen = seen;
            Deleted = deleted;
        }

        public HomeRecivedMessagesViewModel()
        {
        }
    }
}
