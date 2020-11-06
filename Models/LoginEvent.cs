
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Community.Models
{
    public class LoginEvent
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public DateTime LoginDate { get; set; }
        [ForeignKey("IdentityUser"), Required]
        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }
        private LoginEvent(){}
        public LoginEvent(string id, DateTime loginDate, string userId)
        {
            Id = id;
            LoginDate = loginDate;
            UserId = userId;
        }
    }
}
