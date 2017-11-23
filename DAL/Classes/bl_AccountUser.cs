using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using DAL.Models;

namespace DAL.Classes
{
    public class bl_UserAccount
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

        public void Add(bl_UserAccount info)
        {
            using (OurDBContext db = new OurDBContext())
            {
                db.UserAccount.Add(info);
                db.SaveChanges();
            }
        }
    }
}
