using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Community.ViewModels
{
    public class HomeInboxViweModel

    {
        [Display(Name = "Sender List")]
        public List<string> Senders { get; set; }
        [Display(Name = "Received messages")]
        public int NumOfMessages { get; set; }
        [Display(Name = "Unseen messages")]
        public int NumOfUnseenMessages { get; set; }
        [Display(Name = "Deleted Messages")]
        public int NumOfDeletedMessages { get; set; }

        public HomeInboxViweModel(List<string> senders, int numOfMessages, int numOfUnseenMessages, int numOfDeletedMessages)
        {
            Senders = senders;
            NumOfMessages = numOfMessages;
            NumOfUnseenMessages = numOfUnseenMessages;
            NumOfDeletedMessages = numOfDeletedMessages;
        }
        public HomeInboxViweModel()
        {

        }
    }
}
