using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Community.ViewModels
{
    public class HomeComposeViewModel
    {
        public string Id { get; set; }
        public string Recipient { get; set; }
        public DateTime SendingDate { get; set; }

        public HomeComposeViewModel(string id, string recipient, DateTime sendingDate)
        {
            Id = id;
            Recipient = recipient;
            SendingDate = sendingDate;
        }

        public HomeComposeViewModel()
        {
            Id = null;
            Recipient = null;
        }



        public override string ToString()
        {
            return "Id " + Id;
        }
    }
}
