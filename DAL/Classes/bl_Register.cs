using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Classes
{
    public class bl_Register
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public static long Register(bl_Register info)
        {

            //Validation



            //Crypto



            //db add
            using (var metadata = new db_VoidConFormworkEntities())
            {

                var User = new db_Users
                {
                    Email = info.Email,
                    Password = info.Password,
                    //FirstName = info.FirstName,
                    //LastName = info.LastName
                    
                };

                metadata.db_Users.Add(User);

                metadata.SaveChanges();

                return User.userID;
            }
        }

        public static bool Login(string Username, string Password)
        {
            using (var metadata = new db_VoidConFormworkEntities())
            {
                //get user using LINQ
                var User =  metadata.db_Users.Where(r => r.Username == Username && r.isActive).FirstOrDefault();

                if(User == null)
                {
                    //fail
                }

                //Crypto

                var isAuth = true;

                if (isAuth)
                    return true;
                else
                    return false;

            }
        }
    }
}
