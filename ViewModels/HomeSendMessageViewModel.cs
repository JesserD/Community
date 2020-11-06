using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Community.ViewModels
{
    public class HomeSendMessageViewModel
    {
        [StringLength(128, MinimumLength = 4, ErrorMessage = "Allowed length 1-128"),
            Required(ErrorMessage = "Subject is required.")]
        public string Subject { get; set; }
        [StringLength(3000, MinimumLength = 1, ErrorMessage = "Allowed length 1-3000"),
            Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; }
        [Required(ErrorMessage = "Sender is required."), EmailAddress]
        public string Sender { get; set; }
        [Required(ErrorMessage = "Recipient is required."), EmailAddress]
        public string Recipient { get; set; }

        public override string ToString()
        {
            return this.Sender + " " + this.Recipient + " " + this.Subject + " " + this.Content;
        }
    }
}
