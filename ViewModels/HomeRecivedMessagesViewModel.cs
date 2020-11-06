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
        public string Sender { get; set; }
        public string Subject { get; set; }
        public string SendingDate { get; set; }
        [Required]
        public bool Seen { get; set; }
        [Required]
        public bool Deleted { get; set; }

        public HomeRecivedMessagesViewModel(string id, string sender, string subject, string sendingDate, bool seen, bool deleted)
        {
            Id = id;
            Sender = sender;
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
