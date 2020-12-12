using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Community.ViewModels
{
    public class HomeIndexViewModel
    {
        [Display(Name = "The last login:")]
        public DateTime LastLogin { get; set; }
        [Display(Name = "The number of logins in the last 30 days:")]
        public int NumOfLoginsLastMonth { get; set; }
        [Display(Name = "The number of unseen messages:")]
        public int NumOfUnseenMessages { get; set; }

        public HomeIndexViewModel (DateTime _lastLogin, int _numOfLoginsLastMonth, int _numOfUnseenMessages)
        {
            this.LastLogin = _lastLogin;
            this.NumOfLoginsLastMonth = _numOfLoginsLastMonth;
            this.NumOfUnseenMessages = _numOfUnseenMessages;
        }

        public HomeIndexViewModel() { }
    }
}
