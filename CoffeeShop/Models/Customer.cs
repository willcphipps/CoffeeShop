using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShop.Models {
    public class Customer {

        [Key]
        public int UserId { get; set; }
        public string UserType { get; set; }

        [Required]
        [MinLength (2, ErrorMessage = "First name must be longer than 2!")]
        [Display (Name = "First Name : ")]
        public string FirstName { get; set; }

        [Required]
        [MinLength (2, ErrorMessage = "Last name must be longer than 2!")]
        [Display (Name = "Last Name : ")]
        public string LastName { get; set; }

        [EmailAddress (ErrorMessage = "Must be valid Email Address")]
        [Required (ErrorMessage = "Must Enter Email")]
        [Display (Name = "Email Address : ")]
        public string Email { get; set; }

        [DataType (DataType.Password)]
        [Required]
        [MinLength (8, ErrorMessage = "Password must be 8 characters or longer!")]
        [Display (Name = "Password : ")]
        public string Password { get; set; }

        [NotMapped]
        [Compare ("Password")]
        [DataType (DataType.Password)]
        [Display (Name = "Confirm Pasword : ")]
        public string Confirm { get; set; }
    }
}