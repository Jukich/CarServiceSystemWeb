using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarServiceSystemWeb.CustomValidation;



namespace CarServiceSystemWeb.EntityContext
{
    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        public int UserId { get; set; }
        [DuplicateValidation("UserProfile")]
        [Required]
        [RegularExpression(@"^[A-Z]{1}[a-z]{5,35}$", ErrorMessage = "Username can contain only letters and must start with upper case!")]
        public string UserName { get; set; }
        [Required]
        [RegularExpression(@"^.{8,35}$", ErrorMessage = "Password must contain minumum 8 characters!")]
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }
}