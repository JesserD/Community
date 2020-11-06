using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Community.ViewModels
{
    public class HomeViewMessageViewModel
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string Subject { get; set; }
        public string Sender { get; set; }
        public DateTime SendingDate { get; set; }

        public HomeViewMessageViewModel(string id, string content, string subject, string sender, DateTime sendingDate)
        {
            Id = id;
            Content = content;
            Subject = subject;
            Sender = sender;
            SendingDate = sendingDate;
        }
        public HomeViewMessageViewModel()
        {
        }
    }
}