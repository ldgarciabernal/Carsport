using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Carsport.Backend.Models
{
    public class UserView
    {
        public string UserId { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        public string EMail { get; set; }

        [Display(Name = "Profile image")]
        public string ImagePath { get; set; }

        [Display(Name = "Is cinfirmed")]
        public bool EmailConfirmed { get; set; }
    }
}