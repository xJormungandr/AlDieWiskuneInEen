using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VoidConFormwork.Models
{
    public class UserAccount
    {
        [Key]
        public long UserID { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        [Required(ErrorMessage = "Username is Required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is Required")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}