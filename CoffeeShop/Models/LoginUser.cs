using System;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Models {
    public class LoginUser {
        [Required]
        [EmailAddress]
        [Display (Name = "Email Address : ")]
        public string LoginEmail { get; set; }

        [DataType (DataType.Password)]
        [Required]
        [Display (Name = "Password : ")]
        public string LoginPassword { get; set; }

    }
}