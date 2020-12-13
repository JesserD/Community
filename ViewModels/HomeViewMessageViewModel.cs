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
        public string SenderId { get; set; }
        public string SenderEmail { get; set; }
        public DateTime SendingDate { get; set; }

        public HomeViewMessageViewModel(string id, string content, string subject, string senderId, string senderEmail, DateTime sendingDate)
        {
            Id = id;
            Content = content;
            Subject = subject;
            SenderId = senderId;
            SenderEmail = senderEmail;
            SendingDate = sendingDate;
        }
        public HomeViewMessageViewModel()
        {
        }
    }
}